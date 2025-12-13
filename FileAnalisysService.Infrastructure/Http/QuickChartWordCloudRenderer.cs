using System.Net.Http.Json;
using FileAnalisysService.UseCases.Ports;

public class QuickChartWordCloudRenderer : IWordCloudRenderer
{
    private readonly HttpClient _http;

    public QuickChartWordCloudRenderer(HttpClient http)
    {
        _http = http;
    }

    public byte[] RenderPng(string text)
    {
        var request = new
        {
            format = "png",
            width = 800,
            height = 600,
            text = text
        };

        var response = _http
            .PostAsJsonAsync("https://quickchart.io/wordcloud", request)
            .GetAwaiter()
            .GetResult();

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Ошибка генерации word cloud");

        return response.Content
            .ReadAsByteArrayAsync()
            .GetAwaiter()
            .GetResult();
    }
}