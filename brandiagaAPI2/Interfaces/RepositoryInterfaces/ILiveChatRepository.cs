//using brandiagaAPI2.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
//{
//    public interface ILiveChatRepository
//    {
//        Task<LiveChatMessage> GetMessageByIdAsync(Guid messageId);
//        Task<IEnumerable<LiveChatMessage>> GetMessagesByUserIdAsync(Guid userId);
//        Task<IEnumerable<LiveChatMessage>> GetMessagesByAdminIdAsync(Guid adminId);
//        Task<IEnumerable<LiveChatMessage>> GetMessagesByClientIdAsync(string clientId);
//        Task<IEnumerable<LiveChatMessage>> GetAllMessagesAsync();
//        Task AddMessageAsync(LiveChatMessage message);
//        Task UpdateMessageAsync(LiveChatMessage message);
//        Task DeleteMessageAsync(Guid messageId);
//    }
//}