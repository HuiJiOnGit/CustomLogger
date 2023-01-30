namespace SimpleRollingFileLog;

internal class RollingFileProcessor : IDisposable
{
    private readonly PathRoller _pathRoller;

    internal RollingFileProcessor(PathRoller pathRoller)
    {
        _pathRoller = pathRoller;
    }

    internal void WriteMessage(ReadOnlySpan<char> message)
    {
        try
        {
            _pathRoller.GetLogFilePath(DateTime.Now, out string path);
            using FileStream fs = new(path, FileMode.Append, FileAccess.Write);
            using StreamWriter writer = new(fs, System.Text.Encoding.UTF8);
            writer.Write(message);
        }
        catch (IOException)
        {
        }
    }

    public void Dispose()
    {
    }
}