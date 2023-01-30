using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SimpleRollingFileLog;

public static class RollingFileLogExtensions
{
    public static ILoggingBuilder AddSimpleRollingLog(this ILoggingBuilder builder, Action<RollingFileLogOptions>? action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }
        builder.Services.Configure(action);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, RollingFileProvider>());
        return builder;
    }
}