using System.Security.Claims;
using Asp.Versioning;
using HealthTracker.Application.Features.MealLogs.Commands;
using HealthTracker.Application.Features.MealLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/meal-logs")]
[Authorize]
[ApiVersion("1.0")]
public class MealLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MealLogsController(IMediator mediator) => _mediator = mediator;
    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMealLogRequest request)
    {
        var result = await _mediator.Send(new CreateMealLogCommand(
            GetUserId(), request.MealType, request.Description, request.LogDate));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var result = await _mediator.Send(new GetMealLogsQuery(GetUserId(), from, to));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMealLogRequest request)
    {
        var result = await _mediator.Send(new UpdateMealLogCommand(
            id, GetUserId(), request.MealType, request.Description));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMealLogCommand(id, GetUserId()));
        return Ok(result);
    }
}

public record CreateMealLogRequest(string MealType, string Description, DateOnly LogDate);
public record UpdateMealLogRequest(string MealType, string Description);