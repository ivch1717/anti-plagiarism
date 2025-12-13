namespace FileAnalisysService.UseCases.Ports;

public sealed record FileDownloadDto(
    string ContentType,
    byte[] Content
);