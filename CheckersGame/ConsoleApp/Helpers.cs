using Microsoft.Extensions.Logging;

namespace ConsoleApp;

public class Helpers
{
    public static readonly ILoggerFactory MyLoggerFactory =
        LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information)
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning)
                .AddConsole();
        });
}