using ProductInventoryMvc.Exceptions;
using System.Net;

namespace ProductInventoryMvc.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidInputException ex)
            {
                _logger.LogError(ex, ex.ExcMessage);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch (ProductInventoryNotFoundException ex)
            {
                _logger.LogError(ex, ex.ExcMessage);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}
