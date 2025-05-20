using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

builder.Services.AddHttpClient();

var app = builder.Build();
app.MapMcp();
app.Run();

//[McpServerToolType]
//public static class TimeTools
//{
//    [McpServerTool, Description("現在の時刻を取得")]
//    public static string GetCurrentTime()
//    {
//        return DateTimeOffset.Now.ToString();
//    }

//    [McpServerTool, Description("指定されたタイムゾーンの現在の時刻を取得")]
//    public static string GetTimeInTimezone(string timezone)
//    {
//        try
//        {
//            var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
//            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tz).ToString();
//        }
//        catch
//        {
//            return "無効なタイムゾーンが指定されています";
//        }
//    }
//}


[McpServerToolType]
public static class WeatherForecastTool
{
    [McpServerTool, Description("今日の仙台の天気予報を取得")]
    public static async Task<string> GetWeatherForecastToday(HttpClient client)
    {
        // User-Agentを追加
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("aspnet-mcp-server-test", "1.0"));

        // 天気予報API（https://weather.tsukumijima.net/）にHTTP GETリクエストを送信
        HttpResponseMessage response = await client.GetAsync("https://weather.tsukumijima.net/api/forecast?city=040010");

        // レスポンスを確認
        response.EnsureSuccessStatusCode();
        // レスポンスの内容をストリームとして取得
        using Stream content = await response.Content.ReadAsStreamAsync();

        // 天気予報を取得
        using JsonDocument jsonDocument = await JsonDocument.ParseAsync(content);
        string weatherforecasttoday = jsonDocument.RootElement
            .GetProperty("description")
            .GetProperty("bodyText")
            .GetString() ?? string.Empty; // Nullチェック

        return $"本日の仙台の天気予報：{weatherforecasttoday}";
    }



}
