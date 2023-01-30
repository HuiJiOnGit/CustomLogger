using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleRollingFileLog;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("SimpleRollingFile")]
internal class RollingFileProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, RollingFileLogger> _loggers = new();
    private readonly RollingFileProcessor _processor;
    private readonly RollingFileLogOptions _logOptions;

    public RollingFileProvider(IOptions<RollingFileLogOptions> logOptions)
    {
        if (string.IsNullOrEmpty(logOptions.Value.AbsoluteLogDirectory))
        {
            throw new ArgumentNullException(nameof(logOptions.Value.AbsoluteLogDirectory), "can't be null");
        }
        _logOptions = logOptions.Value;
        _processor = new RollingFileProcessor(new PathRoller(_logOptions.AbsoluteLogDirectory, _logOptions.Prefix, _logOptions.Suffix, _logOptions.RollingInterval));
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, new RollingFileLogger(categoryName, _logOptions, _processor));
    }

    public void Dispose()
    {
        _loggers.Clear();
        GC.SuppressFinalize(this);
    }
}