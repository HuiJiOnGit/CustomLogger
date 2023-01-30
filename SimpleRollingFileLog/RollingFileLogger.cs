using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace SimpleRollingFileLog;

internal class RollingFileLogger : ILogger
{
    private readonly RollingFileProcessor _processor;
    private const string _colonPadding = ": ";
    private const string _spacePadding = " ";
    private static readonly string _messagePadding = new(' ', GetLogLevelString(LogLevel.Information).Length + _colonPadding.Length);

    [ThreadStatic]
    private static StringWriter? _stringWriter;

    public string CategoryName { get; }
    public string NewLine { get; private set; }
    public RollingFileLogOptions LogOptions { get; }

    public RollingFileLogger(string categoryName, RollingFileLogOptions logOptions, RollingFileProcessor processor)
    {
        CategoryName = categoryName;
        LogOptions = logOptions;
        _processor = processor;
        NewLine = Environment.NewLine;
    }

    /// <summary>
    /// <inheritdoc />
    /// <para>输出的内容格式如下:</para>
    /// <para>yyyy-MM-dd HH:mm:ss LogLevel CategoryName [EventId]:<para></para>
    /// <para>message</para>
    /// <para>Exception</para>
    /// </para>
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        _stringWriter ??= new StringWriter();
        if (LogOptions.LogLogLevel)
        {
            _stringWriter.Write(GetLogLevelString(logLevel));
            _stringWriter.Write(_colonPadding);
        }
        string? timestamp = null;
        string? timestampFormat = LogOptions.TimestampFormat;
        if (timestampFormat is not null)
        {
            DateTimeOffset dateTimeOffset = GetCurrentDateTime();
            timestamp = dateTimeOffset.ToString(timestampFormat);
        }
        if (timestamp is not null)
        {
            _stringWriter.Write(timestamp);
            _stringWriter.Write(_spacePadding);
        }
        _stringWriter.Write(CategoryName);
        _stringWriter.Write("[");
        _stringWriter.Write(eventId.ToString());
        _stringWriter.Write("]");
        _stringWriter.Write(_colonPadding);
        _stringWriter.Write(NewLine);
        _stringWriter.Write(_messagePadding);
        _stringWriter.Write(formatter(state, exception));
        _stringWriter.Write(NewLine);
        if (exception is not null)
        {
            _stringWriter.Write(exception.ToString());
            _stringWriter.Write(NewLine);
        }
        var sb = _stringWriter.GetStringBuilder();
        if (sb.Length == 0)
        {
            return;
        }
        string computedAnsiString = sb.ToString();
        sb.Clear();
        if (sb.Capacity > 1024)
        {
            sb.Capacity = 1024;
        }
        _processor.WriteMessage(computedAnsiString);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => ""
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private DateTimeOffset GetCurrentDateTime()
    {
        return LogOptions.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
    }
}