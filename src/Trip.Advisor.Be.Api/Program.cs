using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Trip.Advisor.Be.Api.Configurantions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.RegisterServices(builder.Configuration);


var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseApiConfig(builder.Environment);
app.UseSwaggerConfig(provider);

Console.WriteLine("===========================================================================");
Console.WriteLine("TRIP ADVISOR BACKEND STARTING.");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine("===========================================================================");

app.Run();

