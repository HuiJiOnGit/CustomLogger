﻿using Microsoft.Extensions.Logging;

namespace ColorConsole;

public sealed class ColorConsoleLoggerConfiguration
{
    public int EventId { get; set; }

    public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
    {
        [LogLevel.Information] = ConsoleColor.Green
    };
}