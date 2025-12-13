using FileAnalisysService.Infrastructure;
using FileAnalisysService.Presentation;
using FileAnalisysService.UseCases.SubmitWork;
using FileAnalisysService.UseCases.GetReportsByWorkId;
using FileAnalisysService.UseCases.GetReportsByAssignment;
using FileAnalysisService.Infrastructure.Http;

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
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("FileStoringService base URL is missing.");
    }
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapAnalysisEndpoints();

app.Run();