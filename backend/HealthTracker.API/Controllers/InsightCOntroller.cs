using System.Security.Claims;
using Asp.Versioning;
using HealthTracker.Application.Features.Insights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/insights")]
[Authorize]
[ApiVersion("1.0")]
public class InsightsController : ControllerBase
{
    private readonly IMediator _mediator;
    public InsightsController(IMediator mediator) => _mediator = mediator;
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("correlations")]
    [EnableRateLimiting("insights")]
    public async Task<IActionResult> GetCorrelations(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var dateFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var dateTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await _mediator.Send(
            new GetHealthInsightsQuery(GetUserId(), dateFrom, dateTo));
        return Ok(result);
    }
}