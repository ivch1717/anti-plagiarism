using System.Net.Http.Json;
using FileAnalisysService.UseCases.SubmitWork;

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

    private record HashDto(string Hash);
}