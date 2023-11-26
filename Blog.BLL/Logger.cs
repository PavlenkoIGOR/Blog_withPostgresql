using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL
{
    public interface IMyLogger
    {
        public Task WriteEvent(string eventMessage);
        public Task WriteError(string errorMessage);
    }

    public class MyLogger : IMyLogger
    {
        IWebHostEnvironment _env;
        public MyLogger(IWebHostEnvironment env) { _env = env; }
        public async Task WriteEvent(string eventMessage)
        {
            Console.WriteLine(eventMessage);
            //await WriteActions.CreateLogFolder_File(_env, "Logger", $"{eventMessage}");
        }
        public async Task WriteError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            //await WriteActions.CreateLogFolder_File(_env, "ErrorsLogger", $"{errorMessage}");
        }
    }

}
