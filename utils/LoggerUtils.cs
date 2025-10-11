using Microsoft.Extensions.Logging;

namespace utils;

public static class LoggerUtils
{

    public static ILoggerFactory Factory = LoggerFactory.Create(builder => builder.AddConsole());
    
}