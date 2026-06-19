using System.Security.Claims;
using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Application.Features.MoodLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/mood-logs")]
[Authorize]
public class MoodLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MoodLogsController(IMediator mediator) => _mediator = mediator;
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMoodLogRequest request)
    {
        var result = await _mediator.Send(new CreateMoodLogCommand(GetUserId(), request.MoodScore, request.Notes, request.LogDate));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var result = await _mediator.Send(new GetMoodLogsQuery(GetUserId(), from, to));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMoodLogRequest request)
    {
        var result = await _mediator.Send(new UpdateMoodLogCommand(id, GetUserId(), request.MoodScore, request.Notes));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMoodLogCommand(id, GetUserId()));
        return Ok(result);
    }
}

public record CreateMoodLogRequest(int MoodScore, string? Notes, DateOnly LogDate);
public record UpdateMoodLogRequest(int MoodScore, string? Notes);