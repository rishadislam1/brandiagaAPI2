using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace brandiagaAPI2.Hubs
{
    public class LiveChatHub : Hub
    {
        private readonly ILogger<LiveChatHub> _logger;

        public LiveChatHub(ILogger<LiveChatHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                _logger.LogWarning("Invalid userId format: {UserId}", userId);
                throw new ArgumentException("Invalid user ID format.", nameof(userId));
            }

            var target = Clients.User(userGuid.ToString());
            if (target != null)
            {
                await target.SendAsync("ReceiveMessage", message, userGuid.ToString());
                _logger.LogInformation("Message sent to user {UserId} from {SenderId}: {Message}", userGuid, Context.ConnectionId, message);
            }
            else
            {
                _logger.LogWarning("No client connected for user {UserId}", userGuid);
                throw new HubException($"No connected client found for user {userGuid}.");
            }
        }

        public async Task SendMessageToAdmin(string adminId, string message)
        {
            if (!Guid.TryParse(adminId, out Guid adminGuid))
            {
                _logger.LogError("Invalid adminId format: {AdminId}", adminId);
                throw new ArgumentException("Invalid admin ID format.", nameof(adminId));
            }

            var target = Clients.User(adminGuid.ToString());
            if (target != null)
            {
                await target.SendAsync("ReceiveMessage", message, Context.UserIdentifier ?? Context.ConnectionId);
                _logger.LogInformation("Message sent to admin {AdminId} from {SenderId}: {Message}", adminGuid, Context.UserIdentifier, message);
            }
            else
            {
                _logger.LogWarning("No client connected for admin {AdminId}", adminGuid);
                throw new HubException($"No connected client found for admin {adminGuid}.");
            }
        }

        public async Task BroadcastMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, Context.UserIdentifier ?? Context.ConnectionId);
        }

        public async Task UpdateMessage(Guid messageId, string message)
        {
            await Clients.All.SendAsync("UpdateMessage", messageId.ToString(), message);
        }

        public async Task DeleteMessage(Guid messageId)
        {
            await Clients.All.SendAsync("DeleteMessage", messageId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected with ConnectionId: {ConnectionId}", Context.ConnectionId);
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected with ConnectionId: {ConnectionId}", Context.ConnectionId);
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task JoinAdminGroup()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
        }
    }
}