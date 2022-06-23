using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogSampleDemo001
{
    public class MyClass : IMyInterface
    {
        private readonly ILogger<MyClass> logger;

        public MyClass(ILogger<MyClass> logger)
        {
            this.logger = logger;
            logger.LogDebug("Constructor Invoked.");
        }

        public void PrintHello()
        {
            logger.LogDebug("PrintHello Invoked.");
            Console.WriteLine("Hello!");
        }
    }
}
