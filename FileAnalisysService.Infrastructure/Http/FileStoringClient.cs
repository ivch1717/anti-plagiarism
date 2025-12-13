using System.Net.Http.Json;
using FileAnalisysService.UseCases.Ports;

namespace FileAnalysisService.Infrastructure.Http;

public class FileStoringClient : IFileStoringClient
{
    private readonly HttpClient _httpClient;

    public FileStoringClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string GetFileHash(Guid fileId)
    {
        var response = _httpClient
            .GetFromJsonAsync<HashDto>($"/files/{fileId}/hash")
            .GetAwaiter()
            .GetResult();

        if (response == null || string.IsNullOrWhiteSpace(response.Hash))
            throw new Exception("File hash not found or invalid response from FileStoringService.");

        return response.Hash;
    }

    public FileDownloadDto Download(Guid fileId)
    {
        var resp = _httpClient
            .GetAsync($"/files/{fileId}", HttpCompletionOption.ResponseHeadersRead)
            .GetAwaiter()
            .GetResult();

        if (!resp.IsSuccessStatusCode)
        {
            var err = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new Exception($"File download failed: {(int)resp.StatusCode} {err}");
        }

        var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
        var contentType = resp.Content.Headers.ContentType?.ToString();

        return new FileDownloadDto(contentType, bytes);
    }

    private record HashDto(string Hash);
}