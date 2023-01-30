using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace ColorConsole;

public static class ColorConsoleLoggerExtensions
{
    public static ILoggingBuilder AddColorConsoleLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);
        builder.SetDefaultColorConsoleConfiguration(null);
        return builder;
    }

    public static ILoggingBuilder AddColorConsoleLogger(
        this ILoggingBuilder builder,
        Action<ColorConsoleLoggerConfiguration> configure)
    {
        builder.AddColorConsoleLogger();
        builder.SetDefaultColorConsoleConfiguration(configure);
        return builder;
    }

    private static void SetDefaultColorConsoleConfiguration(this ILoggingBuilder builder, Action<ColorConsoleLoggerConfiguration>? configure)
    {
        if (configure is null)
        {
            configure = configuration =>
            {
                configuration.LogLevelToColorMap[LogLevel.Trace] = ConsoleColor.DarkGray;
                configuration.LogLevelToColorMap[LogLevel.Debug] = ConsoleColor.DarkCyan;
                configuration.LogLevelToColorMap[LogLevel.Information] = ConsoleColor.DarkGreen;
                configuration.LogLevelToColorMap[LogLevel.Warning] = ConsoleColor.DarkYellow;
                configuration.LogLevelToColorMap[LogLevel.Error] = ConsoleColor.Red;
                configuration.LogLevelToColorMap[LogLevel.Critical] = ConsoleColor.DarkRed;
            };
        }
        builder.Services.Configure(configure);
    }
}