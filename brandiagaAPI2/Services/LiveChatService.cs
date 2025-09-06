//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Dtos;
//using brandiagaAPI2.Hubs;
//using brandiagaAPI2.Interfaces;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using brandiagaAPI2.Interfaces.ServiceInterfaces;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Services
//{
//    public class LiveChatService : ILiveChatService
//    {
//        private readonly ILiveChatRepository _liveChatRepository;
//        private readonly IHubContext<LiveChatHub> _hubContext;
//        private readonly INotificationService _notificationService;


//        public LiveChatService(
//    ILiveChatRepository liveChatRepository,
//    IHubContext<LiveChatHub> hubContext,
//    INotificationService notificationService)
//        {
//            _liveChatRepository = liveChatRepository;
//            _hubContext = hubContext;
//            _notificationService = notificationService;
//        }

//        public async Task<LiveChatMessageResponseDto> GetMessageByIdAsync(Guid messageId)
//        {
//            var message = await _liveChatRepository.GetMessageByIdAsync(messageId);
//            if (message == null) throw new Exception("Message not found");

//            return new LiveChatMessageResponseDto
//            {
//                MessageId = message.MessageId,
//                UserId = message.UserId,
//                AdminId = message.AdminId,
//                ClientId = message.ClientId,
//                Message = message.Message,
//                SentAt = message.SentAt,
//                IsRead = message.IsRead
//            };
//        }

//        public async Task<IEnumerable<LiveChatMessageResponseDto>> GetMessagesByUserIdAsync(Guid userId)
//        {
//            var messages = await _liveChatRepository.GetMessagesByUserIdAsync(userId);
//            return messages.Select(m => new LiveChatMessageResponseDto
//            {
//                MessageId = m.MessageId,
//                UserId = m.UserId,
//                AdminId = m.AdminId,
//                ClientId = m.ClientId,
//                Message = m.Message,
//                SentAt = m.SentAt,
//                IsRead = m.IsRead
//            }).ToList();
//        }

//        public async Task<IEnumerable<LiveChatMessageResponseDto>> GetMessagesByAdminIdAsync(Guid adminId)
//        {
//            var messages = await _liveChatRepository.GetMessagesByAdminIdAsync(adminId);
//            return messages.Select(m => new LiveChatMessageResponseDto
//            {
//                MessageId = m.MessageId,
//                UserId = m.UserId,
//                AdminId = m.AdminId,
//                ClientId = m.ClientId,
//                Message = m.Message,
//                SentAt = m.SentAt,
//                IsRead = m.IsRead
//            }).ToList();
//        }

//        public async Task<IEnumerable<LiveChatMessageResponseDto>> GetMessagesByClientIdAsync(string clientId)
//        {
//            var messages = await _liveChatRepository.GetMessagesByClientIdAsync(clientId);
//            return messages.Select(m => new LiveChatMessageResponseDto
//            {
//                MessageId = m.MessageId,
//                UserId = m.UserId,
//                AdminId = m.AdminId,
//                ClientId = m.ClientId,
//                Message = m.Message,
//                SentAt = m.SentAt,
//                IsRead = m.IsRead
//            }).ToList();
//        }

//        public async Task<IEnumerable<LiveChatMessageResponseDto>> GetAllMessagesAsync()
//        {
//            var messages = await _liveChatRepository.GetAllMessagesAsync();
//            return messages.Select(m => new LiveChatMessageResponseDto
//            {
//                MessageId = m.MessageId,
//                UserId = m.UserId,
//                AdminId = m.AdminId,
//                ClientId = m.ClientId,
//                Message = m.Message,
//                SentAt = m.SentAt,
//                IsRead = m.IsRead
//            }).ToList();
//        }

//        public async Task<LiveChatMessageResponseDto> SendMessageAsync(LiveChatMessageRequestDto messageDto)
//        {
//            var clientId = messageDto.ClientId ?? Guid.NewGuid().ToString(); // Generate clientId for anonymous users

//            var message = new Data.Models.LiveChatMessage
//            {
//                MessageId = Guid.NewGuid(),
//                UserId = messageDto.UserId, // Nullable for anonymous users
//                AdminId = messageDto.AdminId,
//                ClientId = clientId,
//                Message = messageDto.Message,
//                IsRead = messageDto.IsRead ?? false,
//                SentAt = DateTime.UtcNow
//            };

//            await _liveChatRepository.AddMessageAsync(message);

//            // Notify via SignalR
//            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveMessage", message.Message, clientId);
//            await _hubContext.Clients.Client(clientId).SendAsync("ReceiveMessage", message.Message, clientId);

//            // Create notification for admins
//            await _notificationService.CreateNotificationAsync(new NotificationRequestDto
//            {
//                UserId = message.AdminId ?? Guid.Empty, // Default to empty if no admin specified
//                Type = "Message",
//                Message = $"New message from {(message.UserId.HasValue ? $"user {message.UserId}" : $"anonymous client {clientId}")}: {message.Message}"
//            });

//            return new LiveChatMessageResponseDto
//            {
//                MessageId = message.MessageId,
//                UserId = message.UserId,
//                AdminId = message.AdminId,
//                ClientId = message.ClientId,
//                Message = message.Message,
//                SentAt = message.SentAt,
//                IsRead = message.IsRead
//            };
//        }

//        public async Task UpdateMessageAsync(Guid messageId, LiveChatMessageRequestDto messageDto)
//        {
//            var message = await _liveChatRepository.GetMessageByIdAsync(messageId);
//            if (message == null) throw new Exception("Message not found");

//            message.UserId = messageDto.UserId;
//            message.AdminId = messageDto.AdminId;
//            message.ClientId = messageDto.ClientId ?? message.ClientId;
//            message.Message = messageDto.Message;
//            message.IsRead = messageDto.IsRead ?? message.IsRead;

//            await _liveChatRepository.UpdateMessageAsync(message);
//            await _hubContext.Clients.Group("Admins").SendAsync("UpdateMessage", messageId.ToString(), message.Message);
//            if (message.ClientId != null)
//            {
//                await _hubContext.Clients.Client(message.ClientId).SendAsync("UpdateMessage", messageId.ToString(), message.Message);
//            }
//        }

//        public async Task DeleteMessageAsync(Guid messageId)
//        {
//            var message = await _liveChatRepository.GetMessageByIdAsync(messageId);
//            if (message == null) throw new Exception("Message not found");

//            await _liveChatRepository.DeleteMessageAsync(messageId);
//            await _hubContext.Clients.Group("Admins").SendAsync("DeleteMessage", messageId.ToString());
//            if (message.ClientId != null)
//            {
//                await _hubContext.Clients.Client(message.ClientId).SendAsync("DeleteMessage", messageId.ToString());
//            }
//        }
//    }
//}
