using System.Security.Claims;
using Asp.Versioning;
using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Application.Features.SymptomLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/symptom-logs")]
[Authorize]
[ApiVersion("1.0")]
public class SymptomLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SymptomLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSymptomLogRequest request)
    {
        var command = new CreateSymptomLogCommand(
            GetUserId(),
            request.SymptomName,
            request.Severity,
            request.Notes,
            request.LogDate
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var result = await _mediator.Send(
            new GetSymptomLogsQuery(GetUserId(), from, to));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetSymptomLogByIdQuery(id, GetUserId()));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSymptomLogRequest request)
    {
        var command = new UpdateSymptomLogCommand(
            id,
            GetUserId(),
            request.SymptomName,
            request.Severity,
            request.Notes
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteSymptomLogCommand(id, GetUserId()));
        return Ok(result);
    }
}

// Request models — keeps controller inputs separate from Commands
public record CreateSymptomLogRequest(
    string SymptomName,
    int Severity,
    string? Notes,
    DateOnly LogDate
);

public record UpdateSymptomLogRequest(
    string SymptomName,
    int Severity,
    string? Notes
);