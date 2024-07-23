using System.Net;
using System.Text.Json;
using System.Text;

namespace  Trip.Advisor.Be.Api.Extentions;

public class AntiXssMiddleware
{
    private readonly RequestDelegate _next;
    private ErrorResponse _error;
    private readonly int _statusCode = (int)HttpStatusCode.BadRequest;
    private readonly IWebHostEnvironment _environment;

    public AntiXssMiddleware(RequestDelegate next,
                             IWebHostEnvironment environment)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_environment.IsDevelopment())
        {
            context.Response.Headers.Add("Referrer-Policy", "strict-origin");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self'; default-src 'self' https://localhost:7224; font-src *;img-src * data:; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';");
            context.Response.Headers.Add("X-Content-Security-Policy", "frame-ancestors 'self'; default-src 'self'; font-src *;img-src * data:; script-src *; style-src *;");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        }

        if (_environment.IsProduction())
        {
            context.Response.Headers.Add("Referrer-Policy", "strict-origin");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self'; default-src 'self'; font-src *;img-src * data:; script-src *; style-src *;");
            context.Response.Headers.Add("X-Content-Security-Policy", "frame-ancestors 'self'; default-src 'self'; font-src *;img-src * data:; script-src *; style-src *;");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

            if (context.Response.Headers.All(s => s.Key != "Strict-Transport-Security"))
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
            }
        }

        // Check XSS in URL
        if (!string.IsNullOrWhiteSpace(context.Request.Path.Value))
        {
            string url = context.Request.Path.Value;

            if (CrossSiteScriptingValidation.IsDangerousString(url, out _))
            {
                await RespondWithAnError(context).ConfigureAwait(false);
                return;
            }
        }

        // Check XSS in query string
        if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
        {
            string queryString = WebUtility.UrlDecode(context.Request.QueryString.Value);

            if (CrossSiteScriptingValidation.IsDangerousString(queryString, out _))
            {
                await RespondWithAnError(context).ConfigureAwait(false);
                return;
            }
        }

        // Check XSS in request content
        Stream originalBody = context.Request.Body;
        try
        {
            string content = await ReadRequestBody(context);

            if (CrossSiteScriptingValidation.IsDangerousString(content, out _))
            {
                await RespondWithAnError(context).ConfigureAwait(false);
                return;
            }
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            context.Request.Body = originalBody;
        }
    }

    private static async Task<string> ReadRequestBody(HttpContext context)
    {
        var buffer = new MemoryStream();
        await context.Request.Body.CopyToAsync(buffer);
        context.Request.Body = buffer;
        buffer.Position = 0;

        Encoding encoding = Encoding.UTF8;

        string requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
        context.Request.Body.Position = 0;

        return requestContent;
    }

    private async Task RespondWithAnError(HttpContext context)
    {
        context.Response.Clear();
        context.Response.Headers.AddHeaders();
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = _statusCode;

        _error ??= new ErrorResponse
        {
            Description = "Error from AntiXssMiddleware",
            ErrorCode = 500
        };

        await context.Response.WriteAsync(_error.ToJSON());
    }
}

public static class AntiXssMiddlewareExtension
{
    public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<AntiXssMiddleware>();
}

public static class CrossSiteScriptingValidation
{
    private static readonly char[] StartingChars = { '<', '&' };

    #region Public methods

    public static bool IsDangerousString(string s, out int matchIndex)
    {
        matchIndex = 0;

        for (int i = 0; ;)
        {

            // Look for the start of one of our patterns 
            int n = s.IndexOfAny(StartingChars, i);

            // If not found, the string is safe
            if (n < 0)
            {
                return false;
            }

            // If it's the last char, it's safe 
            if (n == s.Length - 1)
            {
                return false;
            }

            matchIndex = n;

            switch (s[n])
            {
                case '<':
                    // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                    if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?')
                    {
                        return true;
                    }

                    break;
                case '&':
                    // If the & is followed by a #, it's unsafe (e.g. S) 
                    if (s[n + 1] == '#')
                    {
                        return true;
                    }

                    break;

            }

            // Continue searching
            i = n + 1;
        }
    }

    #endregion

    #region Private methods

    private static bool IsAtoZ(char c) => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';

    #endregion

    public static void AddHeaders(this IHeaderDictionary headers)
    {
        if (headers["P3P"].IsNullOrEmpty())
        {
            headers.Add("P3P", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        }
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
    public static string ToJSON(this object value) => JsonSerializer.Serialize(value);
}