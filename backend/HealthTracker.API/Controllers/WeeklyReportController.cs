using System.Security.Claims;
using HealthTracker.Application.Features.WeeklyReports.Queries;
using HealthTracker.Infrastructure.BackgroundJobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/weekly-reports")]
[Authorize]
public class WeeklyReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly WeeklyReportJob _weeklyReportJob;

    public WeeklyReportsController(IMediator mediator, WeeklyReportJob weeklyReportJob)
    {
        _mediator = mediator;
        _weeklyReportJob = weeklyReportJob;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetWeeklyReportsQuery(GetUserId()));
        return Ok(result);
    }

    // Manual trigger for testing — in production this runs automatically via Hangfire
    [HttpPost("generate")]
    public async Task<IActionResult> TriggerGeneration()
    {
        await _weeklyReportJob.GenerateWeeklyReportsAsync();
        return Ok(new { message = "Weekly reports generated successfully." });
    }
}