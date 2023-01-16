using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using DatingAPI.Errors;

namespace DatingAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        //framework recognises this method name middleware and decides what to do next
        //Httpcontext gives us access to the HTTP request that is being passed in the middleware.
        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);//if there's no exception just move on to the next middleware execution
            }
            catch(Exception ex){
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                 var response = _env.IsDevelopment()
                 ? new APIException(ex.Message , context.Response.StatusCode, ex.StackTrace?.ToString()) :
                  new APIException(ex.Message , context.Response.StatusCode, "Internal Server error");
                 //ex.StackTrace? it means it is optional so as to get null exception here when we do ToString() on it.
                 var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                 var json = JsonSerializer.Serialize(response, options);

                 await context.Response.WriteAsync(json);//Writes the given text to the response body. 

            }

        }
    }
}