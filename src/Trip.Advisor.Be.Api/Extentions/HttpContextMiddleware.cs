using System.Net;
using System.Text;

namespace  Trip.Advisor.Be.Api.Extentions;

public class HttpContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpContextMiddleware> _logger;

    public HttpContextMiddleware(RequestDelegate next,
                                 ILogger<HttpContextMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = new StringBuilder();
        if (string.IsNullOrWhiteSpace(context.Request.Path.Value) is false)
        {
            request.AppendLine($"Path: {context.Request.Path.Value}");
        }

        if (string.IsNullOrWhiteSpace(context.Request.QueryString.Value) is false)
        {
            request.AppendLine($"Query String: {WebUtility.UrlDecode(context.Request.QueryString.Value)}");
        }

        try
        {
            string content = await ReadRequestBody(context);

            request.AppendLine($"Content: {content}");

            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            request.AppendLine($"Exception: {ex.Message}");
            _logger.LogError(request.ToString());
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
}

public class ErrorResponse
{
    public int ErrorCode { get; set; }
    public string Description { get; set; }
}

public static class HttpContextMiddlewareExtension
{
    public static IApplicationBuilder UseHttpContextMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<HttpContextMiddleware>();
}
