using System.Security.Claims;
using Asp.Versioning;
using HealthTracker.Application.Features.SleepLogs.Commands;
using HealthTracker.Application.Features.SleepLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/sleep-logs")]
[Authorize]
[ApiVersion("1.0")]
public class SleepLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SleepLogsController(IMediator mediator) => _mediator = mediator;
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSleepLogRequest request)
    {
        var result = await _mediator.Send(new CreateSleepLogCommand(
            GetUserId(), request.HoursSlept, request.QualityScore,
            request.BedTime, request.WakeTime, request.LogDate));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var result = await _mediator.Send(new GetSleepLogsQuery(GetUserId(), from, to));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSleepLogRequest request)
    {
        var result = await _mediator.Send(new UpdateSleepLogCommand(
            id, GetUserId(), request.HoursSlept, request.QualityScore,
            request.BedTime, request.WakeTime));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteSleepLogCommand(id, GetUserId()));
        return Ok(result);
    }
}

public record CreateSleepLogRequest(float HoursSlept, int QualityScore, TimeOnly BedTime, TimeOnly WakeTime, DateOnly LogDate);
public record UpdateSleepLogRequest(float HoursSlept, int QualityScore, TimeOnly BedTime, TimeOnly WakeTime);