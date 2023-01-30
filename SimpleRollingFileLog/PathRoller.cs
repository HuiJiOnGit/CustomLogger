namespace SimpleRollingFileLog;

internal class PathRoller
{
    private readonly string _absoluteLogDirectoryPath;
    private readonly string _periodFormat;
    private readonly string _filenamePrefix;
    private readonly string _filenameSuffix;

    internal PathRoller(string absoluteLogDirectoryPath, string? prefix, string suffix, RollingInterval interval)
    {
        if (absoluteLogDirectoryPath == null) throw new ArgumentNullException(nameof(absoluteLogDirectoryPath));
        if (!Directory.Exists(absoluteLogDirectoryPath)) throw new DirectoryNotFoundException(nameof(absoluteLogDirectoryPath));
        _absoluteLogDirectoryPath = absoluteLogDirectoryPath;
        _periodFormat = interval.GetFormat();
        _filenamePrefix = prefix ??= "";
        _filenameSuffix = suffix ??= ".txt";
    }

    internal void GetLogFilePath(DateTime date, out string path)
    {
        string fileName = date.ToString(_periodFormat);
        path = Path.Combine(_absoluteLogDirectoryPath, _filenamePrefix + fileName + _filenameSuffix);
    }
}