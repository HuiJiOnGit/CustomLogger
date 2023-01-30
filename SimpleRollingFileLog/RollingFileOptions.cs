using System.Diagnostics.CodeAnalysis;

namespace SimpleRollingFileLog;

public class RollingFileLogOptions
{
    /// <summary>
    /// 日志输出中的时间的格式化形式
    /// attr read https://learn.microsoft.com/zh-cn/dotnet/core/whats-new/dotnet-7#net-libraries
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.DateTimeFormat)]
    public string TimestampFormat { get; set; } = "G";

    public bool UseUtcTimestamp { get; set; }

    /// <summary>
    /// 输出日志等级
    /// </summary>
    public bool LogLogLevel { get; set; } = true;

    /// <summary>
    /// log保存的绝对路径
    /// </summary>
    public string AbsoluteLogDirectory { get; set; } = null!;

    /// <summary>
    /// log文件的前缀
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// log文件的后缀
    /// </summary>
    public string Suffix { get; set; } = ".txt";

    public RollingInterval RollingInterval { get; set; } = RollingInterval.Month;
}