using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("file-storing", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:FileStoring:BaseUrl"]
        ?? throw new InvalidOperationException("Services:FileStoring:BaseUrl is missing.")
    );
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient("file-analysis", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:FileAnalysis:BaseUrl"]
        ?? throw new InvalidOperationException("Services:FileAnalysis:BaseUrl is missing.")
    );
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

static IResult ServiceUnavailable(string serviceName, Exception? ex = null)
{
    var msg = ex?.Message;
    return Results.Json(
        new { error = $"{serviceName} is unavailable", details = msg },
        statusCode: StatusCodes.Status503ServiceUnavailable
    );
}

static async Task<IResult> ProxyJsonLikeResponse(HttpResponseMessage resp, CancellationToken ct)
{
    var body = await resp.Content.ReadAsStringAsync(ct);
    var contentType = resp.Content.Headers.ContentType?.ToString() ?? "application/json";
    return Results.Content(body, contentType, statusCode: (int)resp.StatusCode);
}

app.MapPost("/works", async (HttpRequest request, IHttpClientFactory factory, CancellationToken ct) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest(new { error = "multipart/form-data expected" });

    IFormCollection form;
    try
    {
        form = await request.ReadFormAsync(ct);
    }
    catch (BadHttpRequestException)
    {
        return Results.BadRequest(new { error = "Invalid multipart/form-data" });
    }

    if (!Guid.TryParse(form["studentId"], out var studentId) || studentId == Guid.Empty)
        return Results.BadRequest(new { error = "studentId is required" });

    if (!Guid.TryParse(form["assignmentId"], out var assignmentId) || assignmentId == Guid.Empty)
        return Results.BadRequest(new { error = "assignmentId is required" });

    var file = form.Files.GetFile("file");
    if (file is null || file.Length == 0)
        return Results.BadRequest(new { error = "file is required" });

    var fileStoring = factory.CreateClient("file-storing");
    var fileAnalysis = factory.CreateClient("file-analysis");

    FileUploadResponse? upload;

    try
    {
        using var uploadForm = new MultipartFormDataContent();

        var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
        uploadForm.Add(fileContent, "file", file.FileName);

        using var uploadResp = await fileStoring.PostAsync("/files", uploadForm, ct);
        if (!uploadResp.IsSuccessStatusCode)
            return await ProxyJsonLikeResponse(uploadResp, ct);

        upload = await uploadResp.Content.ReadFromJsonAsync<FileUploadResponse>(cancellationToken: ct);
        if (upload is null || upload.Id == Guid.Empty)
            return Results.Problem("Invalid file storing response");
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileStoringService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileStoringService (timeout)", ex);
    }

    SubmitWorkResponseDto? analysis;

    try
    {
        var analysisReq = new SubmitWorkRequestDto(studentId, assignmentId, upload!.Id);

        using var analysisResp = await fileAnalysis.PostAsJsonAsync("/works", analysisReq, ct);
        if (!analysisResp.IsSuccessStatusCode)
            return await ProxyJsonLikeResponse(analysisResp, ct);

        analysis = await analysisResp.Content.ReadFromJsonAsync<SubmitWorkResponseDto>(cancellationToken: ct);
        if (analysis is null)
            return Results.Problem("Invalid analysis response");
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileAnalysisService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileAnalysisService (timeout)", ex);
    }

    return Results.Json(new
    {
        fileId = upload!.Id,
        workId = analysis!.WorkId,
        reportId = analysis.ReportId,
        status = analysis.Status
    });
})
.Accepts<SubmitWorkForm>("multipart/form-data")
.WithTags("Gateway / Works")
.DisableAntiforgery();

app.MapGet("/works/{workId:guid}/report", async (Guid workId, IHttpClientFactory factory, CancellationToken ct) =>
{
    var client = factory.CreateClient("file-analysis");
    try
    {
        using var resp = await client.GetAsync($"/works/{workId}/report", ct);
        return await ProxyJsonLikeResponse(resp, ct);
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileAnalysisService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileAnalysisService (timeout)", ex);
    }
})
.WithTags("Gateway / Works");

app.MapGet("/assignments/{assignmentId:guid}/reports", async (Guid assignmentId, IHttpClientFactory factory, CancellationToken ct) =>
{
    var client = factory.CreateClient("file-analysis");
    try
    {
        using var resp = await client.GetAsync($"/assignments/{assignmentId}/reports", ct);
        return await ProxyJsonLikeResponse(resp, ct);
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileAnalysisService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileAnalysisService (timeout)", ex);
    }
})
.WithTags("Gateway / Assignments");

app.MapGet("/files/{fileId:guid}/hash", async (Guid fileId, IHttpClientFactory factory, CancellationToken ct) =>
{
    var client = factory.CreateClient("file-storing");
    try
    {
        using var resp = await client.GetAsync($"/files/{fileId}/hash", ct);
        return await ProxyJsonLikeResponse(resp, ct);
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileStoringService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileStoringService (timeout)", ex);
    }
})
.WithTags("Gateway / Files");

app.MapGet("/files/{fileId:guid}", async (Guid fileId, IHttpClientFactory factory, HttpContext http, CancellationToken ct) =>
{
    var client = factory.CreateClient("file-storing");

    HttpResponseMessage resp;
    try
    {
        resp = await client.GetAsync($"/files/{fileId}", HttpCompletionOption.ResponseHeadersRead, ct);
    }
    catch (HttpRequestException ex)
    {
        return ServiceUnavailable("FileStoringService", ex);
    }
    catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
    {
        return ServiceUnavailable("FileStoringService (timeout)", ex);
    }

    http.Response.RegisterForDispose(resp);

    if (!resp.IsSuccessStatusCode)
    {
        var err = await resp.Content.ReadAsStringAsync(ct);
        return Results.Content(err, "application/json", statusCode: (int)resp.StatusCode);
    }

    var contentType = resp.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

    var fileName =
        resp.Content.Headers.ContentDisposition?.FileNameStar ??
        resp.Content.Headers.ContentDisposition?.FileName ??
        "file";

    fileName = fileName.Trim('"');

    Stream stream;
    try
    {
        stream = await resp.Content.ReadAsStreamAsync(ct);
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = "Failed to read file stream", details = ex.Message }, statusCode: 502);
    }

    return Results.File(stream, contentType, fileName);
})
.WithTags("Gateway / Files");

app.Run();

record SubmitWorkForm(Guid StudentId, Guid AssignmentId, IFormFile File);
record SubmitWorkRequestDto(Guid StudentId, Guid AssignmentId, Guid FileId);
record FileUploadResponse(Guid Id);
record SubmitWorkResponseDto(Guid WorkId, Guid ReportId, int Status);


