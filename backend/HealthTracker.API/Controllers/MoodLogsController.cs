using System.Security.Claims;
using Asp.Versioning;
using HealthTracker.Application.Features.MoodLogs.Commands;
using HealthTracker.Application.Features.MoodLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/mood-logs")]
[Authorize]
[ApiVersion("1.0")]
public class MoodLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;

    public MoodLogsController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMoodLogRequest request)
    {
        var userId = GetUserId();
        var result = await _mediator.Send(new CreateMoodLogCommand(userId, request.MoodScore, request.Notes, request.LogDate));
        _cache.Remove($"mood-logs-{userId}");
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var userId = GetUserId();
        var cacheKey = $"mood-logs-{userId}";
        if (!_cache.TryGetValue(cacheKey, out var cached))
        {
            cached = await _mediator.Send(new GetMoodLogsQuery(userId, from, to));
            _cache.Set(cacheKey, cached, TimeSpan.FromMinutes(3));
        }
        return Ok(cached);
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