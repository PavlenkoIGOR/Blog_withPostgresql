using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL
{
    public class MyLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public MyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///  Необходимо реализовать метод Invoke  или InvokeAsync
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Для логирования данных о запросе используем свойста объекта HttpContext
            Console.WriteLine($"MyLoggingMiddleware: [{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");

            // Передача запроса далее по конвейеру
            await _next.Invoke(context);
        }
    }
}
