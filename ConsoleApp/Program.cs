// See https://aka.ms/new-console-template for more information
using ColorConsole;
using ConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleRollingFileLog;

const string LoggingKey = "Logging";
IServiceCollection services = new ServiceCollection();
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
services.AddLogging(builder =>
{
    // https://github.com/dotnet/runtime/blob/83ce1253cf71348776fec4464e3fad1277fac7f0/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L290
    // 查看源码才知道,要指定"Logging"节点的, 就说设置了没起作用
    builder.AddConfiguration(configuration.GetSection(LoggingKey));
    //builder.AddConsole();
    //builder.AddColorConsoleLogger();
    builder.AddSimpleConsole();
    builder.AddSimpleRollingLog(option =>
    {
        option.AbsoluteLogDirectory = Directory.GetCurrentDirectory();
        option.UseUtcTimestamp= true;
        option.Prefix = "log-";
        option.TimestampFormat = "s";
        option.Suffix = ".log";
        option.LogLogLevel = false;
        option.RollingInterval = RollingInterval.Day;
    });
});

using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var now = DateTime.Now;
logger.LogTrace(0, "[{time:yyyy/MM/dd HH:mm:ss}] Trace", now);
logger.LogDebug(1, "[{time:yyyy/MM/dd HH:mm:ss}] Debug", now);
logger.LogInformation(2, "[{time:yyyy/MM/dd HH:mm:ss}] Info", now);
try
{
    ErrorTest.ThrowError();
}
catch (NotImplementedException ex)
{
    logger.LogWarning(3, ex, "[{time:yyyy/MM/dd HH:mm:ss}] Warning", now);
    logger.LogError(4, ex, "[{time:yyyy/MM/dd HH:mm:ss}] Error", now);
    logger.LogCritical(5, ex, "[{time:yyyy/MM/dd HH:mm:ss}] Critical", now);
}
Console.ReadKey();