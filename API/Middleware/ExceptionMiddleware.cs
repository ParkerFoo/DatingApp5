using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;  //RequestDelegate is what coming up next after middleware pipeline.
        private readonly ILogger<ExceptionMiddleware> _logger; // Ilogger display log at the terminal, 
        private readonly IHostEnvironment _env; //IHostEnvironment is check whether you in prod or dev environmnet

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env) 
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); //process the http request
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);   //display error  
                context.Response.ContentType="application/json"; //response with a type of json
                context.Response.StatusCode=(int) HttpStatusCode.InternalServerError; //response with 500

                var response=_env.IsDevelopment() 
                ? new APIException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                : new APIException(context.Response.StatusCode,"Intenal server error"); 

                var options=new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};

                var json=JsonSerializer.Serialize(response,options); //turning the APIException object into JSON with the option of camel case

                await context.Response.WriteAsync(json); //respond the http with json
            }
        }


    }
}