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
    [McpServerTool, Description("�V�C�\����擾")]
    public static async Task<string> GetWeatherForecastCity(HttpClient client, string citycode)
    {
        // �V�C�\��API�ihttps://weather.tsukumijima.net/�j��HTTP GET���N�G�X�g�𑗐M
        HttpResponseMessage response = await client.GetAsync("https://weather.tsukumijima.net/api/forecast?city=" + citycode);

        // ���X�|���X���m�F
        response.EnsureSuccessStatusCode();

        // ���X�|���X�̓��e���X�g���[���Ƃ��Ď擾
        using Stream content = await response.Content.ReadAsStreamAsync();

        // �V�C�\����擾
        using JsonDocument jsonDocument = await JsonDocument.ParseAsync(content);

        string title = jsonDocument.RootElement
            .GetProperty("title")
            .GetString() ?? string.Empty;

        string weatherforecast = jsonDocument.RootElement
            .GetProperty("description")
            .GetProperty("bodyText")
            .GetString() ?? string.Empty;

        return $"{title}�F{weatherforecast}";
    }
}
