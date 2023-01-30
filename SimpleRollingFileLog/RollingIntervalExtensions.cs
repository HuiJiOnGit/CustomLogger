namespace SimpleRollingFileLog;

internal static class RollingIntervalExtensions
{
    public static string GetFormat(this RollingInterval interval)
    {
        return interval switch
        {
            RollingInterval.Day => "yyyy_MM_dd",
            RollingInterval.Month => "yyyy_MM",
            _ => throw new NotSupportedException("No Support RollingInterval")
        };
    }
}