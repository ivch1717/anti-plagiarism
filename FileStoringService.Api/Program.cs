using FileStoringService.Infrastructure;
using FileStoringService.Presentation;
using FileStoringService.UseCases.DownloadFile;
using FileStoringService.UseCases.UploadFile;
using FileStoringService.UseCases.GetFileHash;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFileStoringInfrastructure(builder.Configuration);

builder.Services.AddScoped<IUploadFileRequestHandler, UploadFileRequestHandler>();
builder.Services.AddScoped<IDownloadFileRequestHandler, DownloadFileRequestHandler>();
builder.Services.AddScoped<IGetFileHashRequestHandler, GetFileHashRequestHandler>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapFilesEndpoints();

app.Run();