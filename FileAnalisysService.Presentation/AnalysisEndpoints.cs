using FileAnalisysService.UseCases.GetReportByWorkId;
using FileAnalisysService.UseCases.GetReportsByAssignment;
using FileAnalisysService.UseCases.SubmitWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FileAnalisysService.Presentation;

public static class AnalysisEndpoints
{
    public static WebApplication MapAnalysisEndpoints(this WebApplication app)
    {
        app.MapGroup("/works")
            .WithTags("Works")
            .MapSubmitWork()
            .MapGetReportByWorkId();

        app.MapGroup("/assignments")
            .WithTags("Assignments")
            .MapGetReportsByAssignment();

        return app;
    }

    private static RouteGroupBuilder MapSubmitWork(this RouteGroupBuilder group)
    {
        group.MapPost("", (SubmitWorkRequest request, ISubmitWorkRequestHandler handler) =>
            {
                var response = handler.Handle(request);
                return Results.Ok(response);
            })
            .WithName("SubmitWork")
            .WithSummary("Submit a work")
            .WithDescription("Create a work submission and run plagiarism check (MVP)")
            .WithOpenApi()
            .Produces<SubmitWorkResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return group;
    }

    private static RouteGroupBuilder MapGetReportByWorkId(this RouteGroupBuilder group)
    {
        group.MapGet("/{workId:guid}/report", (Guid workId, IGetReportByWorkIdRequestHandler handler) =>
            {
                if (workId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный WorkId" });

                var report = handler.Handle(new GetReportsByWorkIdRequest(workId));
                if (report is null)
                    return Results.NotFound(new { error = "Отчет не найден" });

                return Results.Ok(report);
            })
            .WithName("GetReportByWorkId")
            .WithSummary("Get report by work id")
            .WithDescription("Return plagiarism report for a specific work")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static RouteGroupBuilder MapGetReportsByAssignment(this RouteGroupBuilder group)
    {
        group.MapGet("/{assignmentId:guid}/reports", (Guid assignmentId, IGetReportsByAssignmentRequestHandler handler) =>
            {
                if (assignmentId == Guid.Empty)
                    return Results.BadRequest(new { error = "Некорректный AssignmentId" });

                var response = handler.Handle(new GetReportsByAssignmentRequest(assignmentId));
                return Results.Ok(response);
            })
            .WithName("GetReportsByAssignment")
            .WithSummary("Get reports by assignment id")
            .WithDescription("Return all works + reports for a specific assignment")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return group;
    }
}
