using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HealthTracker.API.Hubs;

/// <summary>
/// SignalR hub for real-time push notifications to authenticated clients.
/// Clients connect with their JWT token via the access_token query param.
/// </summary>
[Authorize]
public class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;

    public NotificationsHub(ILogger<NotificationsHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        _logger.LogInformation("Client connected to NotificationsHub. UserId={UserId}", userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        _logger.LogInformation("Client disconnected from NotificationsHub. UserId={UserId}", userId);
        await base.OnDisconnectedAsync(exception);
    }
}
