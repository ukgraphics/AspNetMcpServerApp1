using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();
app.MapMcp();

app.Run();

[McpServerToolType]
public static class TimeTools
{
    [McpServerTool, Description("現在の時刻を取得")]
    public static string GetCurrentTime()
    {
        return DateTimeOffset.Now.ToString();
    }

    [McpServerTool, Description("指定されたタイムゾーンの現在の時刻を取得")]
    public static string GetTimeInTimezone(string timezone)
    {
        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tz).ToString();
        }
        catch
        {
            return "無効なタイムゾーンが指定されています";
        }
    }
}