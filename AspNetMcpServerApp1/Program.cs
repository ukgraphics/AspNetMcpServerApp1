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
//    [McpServerTool, Description("���݂̎������擾")]
//    public static string GetCurrentTime()
//    {
//        return DateTimeOffset.Now.ToString();
//    }

//    [McpServerTool, Description("�w�肳�ꂽ�^�C���]�[���̌��݂̎������擾")]
//    public static string GetTimeInTimezone(string timezone)
//    {
//        try
//        {
//            var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
//            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tz).ToString();
//        }
//        catch
//        {
//            return "�����ȃ^�C���]�[�����w�肳��Ă��܂�";
//        }
//    }
//}


[McpServerToolType]
public static class WeatherForecastTool
{
    [McpServerTool, Description("�����̐��̓V�C�\����擾")]
    public static async Task<string> GetWeatherForecastToday(HttpClient client)
    {
        // User-Agent��ǉ�
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("aspnet-mcp-server-test", "1.0"));

        // �V�C�\��API�ihttps://weather.tsukumijima.net/�j��HTTP GET���N�G�X�g�𑗐M
        HttpResponseMessage response = await client.GetAsync("https://weather.tsukumijima.net/api/forecast?city=040010");

        // ���X�|���X���m�F
        response.EnsureSuccessStatusCode();
        // ���X�|���X�̓��e���X�g���[���Ƃ��Ď擾
        using Stream content = await response.Content.ReadAsStreamAsync();

        // �V�C�\����擾
        using JsonDocument jsonDocument = await JsonDocument.ParseAsync(content);
        string weatherforecasttoday = jsonDocument.RootElement
            .GetProperty("description")
            .GetProperty("bodyText")
            .GetString() ?? string.Empty; // Null�`�F�b�N

        return $"�{���̐��̓V�C�\��F{weatherforecasttoday}";
    }



}
