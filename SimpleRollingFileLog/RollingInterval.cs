namespace SimpleRollingFileLog;

/// <summary>
/// Specifies the frequency at which the log file should roll.
/// </summary>
public enum RollingInterval
{
    /// <summary>
    /// Roll every calendar month. Filenames will have <code>yyyy-MM</code> appended.
    /// </summary>
    Month,

    /// <summary>
    /// Roll every day. Filenames will have <code>yyyy-MM-dd</code> appended.
    /// </summary>
    Day,
}