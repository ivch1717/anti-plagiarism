using FileStoringService.UseCases.DownloadFile;
using FileStoringService.UseCases.UploadFile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FileStoringService.UseCases.GetFileHash;


namespace FileStoringService.Presentation;

public static class FilesEndpoints
{
    public static WebApplication MapFilesEndpoints(this WebApplication app)
    {
        app.MapGroup("/files")
            .WithTags("Files")
            .MapUploadFile()
            .MapDownloadFile()
            .MapGetFileHash();

        return app;
    }

    private static RouteGroupBuilder MapUploadFile(this RouteGroupBuilder group)
    {
        group.MapPost("/", (IFormFile file, IUploadFileRequestHandler handler) =>
            {
                if (file is null || file.Length == 0)
                    return Results.BadRequest(new { error = "Файл не передан или пустой" });

                using var stream = file.OpenReadStream();

                var request = new UploadFileRequest(
                    FileName: file.FileName,
                    ContentType: string.IsNullOrWhiteSpace(file.ContentType)
                        ? "application/octet-stream"
                        : file.ContentType,
                    SizeInBytes: file.Length,
                    Content: stream
                );

                var response = handler.Handle(request);
                
                return Results.Created($"/files/{response.Id}", response);
            })
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<UploadFileResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Upload a file to storage").DisableAntiforgery();

        return group;
    }

    private static RouteGroupBuilder MapDownloadFile(this RouteGroupBuilder group)
    {
        group.MapGet("/{fileId:guid}", (Guid fileId, IDownloadFileRequestHandler handler) =>
            {
                if (fileId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный идентификатор файла" });

                try
                {
                    var response = handler.Handle(new DownloadFileRequest(fileId));
                    
                    return Results.File(
                        fileStream: response.Content,
                        contentType: string.IsNullOrWhiteSpace(response.ContentType)
                            ? "application/octet-stream"
                            : response.ContentType,
                        fileDownloadName: response.FileName
                    );
                }
                catch (FileNotFoundException)
                {
                    return Results.NotFound(new { error = "Файл не найден" });
                }
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Download a file by id")
            .DisableAntiforgery();

        return group;
    }
    
    private static RouteGroupBuilder MapGetFileHash(this RouteGroupBuilder group)
    {
        group.MapGet("/{fileId:guid}/hash", (Guid fileId, IGetFileHashRequestHandler handler) =>
            {
                if (fileId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный идентификатор файла" });

                try
                {
                    var response = handler.Handle(new GetFileHashRequest(fileId));
                    return Results.Ok(response);
                }
                catch (FileNotFoundException)
                {
                    return Results.NotFound(new { error = "Файл не найден" });
                }
            })
            .Produces<GetFileHashResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get file content hash by id")
            .DisableAntiforgery();

        return group;
    }

}
