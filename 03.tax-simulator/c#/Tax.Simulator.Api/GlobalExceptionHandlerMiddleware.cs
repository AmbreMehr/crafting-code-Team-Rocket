namespace Tax.Simulator.Api;

/// <summary>
/// Class that handle exceptions globally
/// </summary>
public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    /// <summary>
    /// Handle exceptions globally
    /// </summary>
    /// <param name="context">context of the request received</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(
                $"An unexpected error occurred. Please try again later.{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}"
            );
        }
    }
}