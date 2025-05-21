using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

builder.Services.AddHttpClient();

var app = builder.Build();
app.MapMcp();
app.Run();


[McpServerToolType]
public static class WeatherForecastTool
{
    [McpServerTool, Description("天気予報を取得")]
    public static async Task<string> GetWeatherForecastCity(HttpClient client, string citycode)
    {
        // 天気予報API（https://weather.tsukumijima.net/）にHTTP GETリクエストを送信
        HttpResponseMessage response = await client.GetAsync("https://weather.tsukumijima.net/api/forecast?city=" + citycode);

        // レスポンスを確認
        response.EnsureSuccessStatusCode();

        // レスポンスの内容をストリームとして取得
        using Stream content = await response.Content.ReadAsStreamAsync();

        // 天気予報を取得
        using JsonDocument jsonDocument = await JsonDocument.ParseAsync(content);

        string title = jsonDocument.RootElement
            .GetProperty("title")
            .GetString() ?? string.Empty;

        string weatherforecast = jsonDocument.RootElement
            .GetProperty("description")
            .GetProperty("bodyText")
            .GetString() ?? string.Empty;

        return $"{title}：{weatherforecast}";
    }
}
