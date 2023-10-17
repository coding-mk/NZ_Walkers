using System.Net;

namespace NZWalksAPI.Middleware;

public class ExceptionHandlerMiddleware
{

  private readonly ILogger<ExceptionHandlerMiddleware> _logger;
  private readonly RequestDelegate _next;

  public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
  RequestDelegate next)
  {
    this._logger = logger;
    this._next = next;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
        {
          await _next(httpContext);
        }
        catch (Exception ex)
        {
          var errorId = Guid.NewGuid();
          // Log This Exception
          _logger.LogError(ex, $"{errorId} : {ex.Message}");

          //Return a custom exception response
          httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          httpContext.Request.ContentType = "application/json";

          var error = new
          {
            errorId = errorId,
            message = "Something went wrong! We are looking for resolving this."
          };

          await httpContext.Response.WriteAsJsonAsync(error);
        }
  }
}