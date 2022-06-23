using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLogSampleDemo001;

var logger = LogManager.GetCurrentClassLogger();

try
{
    var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    var configBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    var nlogConfigFileName = "nlog.config";

    if (!String.IsNullOrEmpty(environment))
    {
        configBuilder.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);
        nlogConfigFileName = $"nlog.{environment}.config";
    }

    var config = configBuilder.Build();


    var servicesProvider = new ServiceCollection()
        .AddTransient<IMyInterface, MyClass>()
       // Add Additional Services i.e bind interfaces to classes
       .AddLogging(loggingBuilder =>
       {
           // configure Logging with NLog
           loggingBuilder.ClearProviders();
           loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
           loggingBuilder.AddNLog(nlogConfigFileName);
       })
       .BuildServiceProvider();

    using(servicesProvider as IDisposable)
    {
        var myInterface = servicesProvider.GetRequiredService<IMyInterface>();
        myInterface.PrintHello();
    }
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}