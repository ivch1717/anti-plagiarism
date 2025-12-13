using FileAnalisysService.Infrastructure;
using FileAnalisysService.Presentation;
using FileAnalisysService.UseCases.SubmitWork;
using FileAnalisysService.UseCases.GetReportByWorkId;
using FileAnalisysService.UseCases.GetReportsByAssignment;
using FileAnalysisService.Infrastructure.Http;
using FileAnalisysService.UseCases.Ports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddFileAnalysisInfrastructure(builder.Configuration);

builder.Services.AddScoped<ISubmitWorkRequestHandler, SubmitWorkRequestHandler>();
builder.Services.AddScoped<IGetReportByWorkIdRequestHandler, GetReportByWorkIdRequestHandler>();
builder.Services.AddScoped<IGetReportsByAssignmentRequestHandler, GetReportsByAssignmentRequestHandler>();

builder.Services.AddHttpClient<IFileStoringClient, FileStoringClient>(client =>
{
    var baseUrl = builder.Configuration["Services:FileStoring:BaseUrl"];
    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("Services:FileStoring:BaseUrl is missing.");

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IWordCloudRenderer, QuickChartWordCloudRenderer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapAnalysisEndpoints();

app.Run();