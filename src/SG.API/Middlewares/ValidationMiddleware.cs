using FluentValidation;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Errors = ex.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                    e.ErrorCode
                })
            });
        }
    }
}