using FluentResults;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using SG.Infrastructure.Auth.Extensions;
using SG.Shared.Settings;
using System.Text;
using System.Text.Json;

namespace SG.API.Middlewares;

public class ErrorHandlerMiddleware
(
    RequestDelegate next,
    IHostEnvironment environment,
    IOptions<AppSettings> settings
)
{
    private  AppSettings _settings = new();

	public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));
        
        _settings = settings.Value;
        if(!_settings.EnableLoggingSerilog)
        {
            await next(httpContext);
            return;
        }


        string requestBody = string.Empty;

        HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);
        Stream body = httpContext.Request.Body;
        byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
        await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
        requestBody = Encoding.UTF8.GetString(buffer);
        body.Seek(0, SeekOrigin.Begin);
        httpContext.Request.Body = body;

        using (var responseBodyMemoryStream = new MemoryStream())
        {
            var originalResponseBodyReference = httpContext.Response.Body;
            httpContext.Response.Body = responseBodyMemoryStream;

            Exception? exx = null;
            try
            {
                await next(httpContext);
            }
            catch (Exception ex) 
            {
                exx = ex;
            }

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            var queryString = httpContext.Request.QueryString;
            var idUser = httpContext?.User?.GetUserIdFromClaims<int>() ?? ((int?)null);
            
            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("UserId", idUser.HasValue ? idUser.Value : "");

            var _logger = Log
                .ForContext("RequestHeaders", httpContext!.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestBody", requestBody)
                .ForContext("ResponseBody", responseBody);

            if(exx != null)
            {
                _logger
                .Error(exx, "Response information {RequestMethod} {RequestPath} {statusCode} {QueryString} , An unexpected exception was thrown: {Message}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode, queryString, exx.Message);
            }
            else
            {
                _logger
                .Debug("Response information {RequestMethod} {RequestPath} {statusCode} {QueryString}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode, queryString);
            }


            if(environment.IsDevelopment() && exx is not null)
            {
                 var result =  Result.Fail(exx.Message);
                await httpContext.Response.WriteAsJsonAsync(JsonSerializer.Serialize(result));
                return; 
            }

            await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
        }
    }
}