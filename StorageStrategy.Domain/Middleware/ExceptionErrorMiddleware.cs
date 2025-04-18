using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.Domain.Middleware
{
    public class ExceptionErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IServiceProvider _serviceProvider;
        public ExceptionErrorMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) 
            {
                using var scope = _serviceProvider.CreateScope();
                var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>();
                
                int userId = context.User.GetUserId();
                await logRepo.AddAsync(new LogApp
                {
                    CreateAt = DateTime.Now,
                    EmployeeId = userId,
                    JsonData = JsonConvert.SerializeObject(new
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    })
                });
                await logRepo.SaveAsync();
                
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            
           
            
            
            
            return context.Response.WriteAsync(result);
        }

    }
}
