using FluentResults;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Context;
using SG.Infrastructure.Auth.Extensions;
using SG.Shared.Helpers;

namespace SG.API.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IConfiguration configuration, IHostEnvironment environment)
    {
        _logger = logger;
        _configuration = configuration;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync
    (
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var settings = new AppConfiguration(_configuration).GetAppSettings();

        if(settings.EnableLoggingGlobalExceptionHandler)
        {
            await GenerateLogError(httpContext, exception);
        }
        
        string errorMessage  = _environment.IsDevelopment() ? exception.Message : UnhandledExceptionMsg;

        var problemDetails =  Result.Fail(errorMessage);
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }

    private async Task GenerateLogError(HttpContext httpContext, Exception exception)
    {  
        var (requestBody, queryString, headers)= await GetInformationRequest(httpContext);
         var idUser = httpContext?.User?.GetUserIdFromClaims<int>() ?? ((int?)null);

        LogContext.PushProperty("UserId", idUser.HasValue ? idUser.Value : "");
        var _logger = Log
            .ForContext("RequestHeaders", headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
            .ForContext("RequestBody", requestBody);

        _logger
            .Error(exception, "Response information {RequestMethod} {RequestPath} {statusCode} {QueryString} , An unexpected exception was thrown: {Message}", 
                    httpContext!.Request.Method, 
                    httpContext.Request.Path, 
                    httpContext.Response.StatusCode, 
                    queryString, 
                    exception.Message);
    }

    private async Task<(string requestBody, QueryString queryString, IHeaderDictionary headers)> GetInformationRequest(HttpContext httpContext)
    {
        string requestBody = await ReadRequestBodyAsync(httpContext.Request);
        var queryString = httpContext.Request.QueryString;
        var headers =  httpContext!.Request.Headers;

        return (requestBody, queryString, headers);
    }

    private  async Task<string> ReadRequestBodyAsync( HttpRequest request)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        request.Body.Position = 0;
        var reader = new StreamReader(request.Body,System.Text.Encoding.UTF8);
        var body = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;
        return body;
    }
}

