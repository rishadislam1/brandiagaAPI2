
//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Interfaces;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;


//namespace brandiagaAPI2.Repositories
//{
//    public class LiveChatRepository : ILiveChatRepository
//    {
//        private readonly DbAbdeaeDotnet1Context _context;

//        public LiveChatRepository(DbAbdeaeDotnet1Context context)
//        {
//            _context = context;
//        }

//        public async Task<LiveChatMessage> GetMessageByIdAsync(Guid messageId)
//        {
//            return await _context.LiveChatMessages
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .FirstOrDefaultAsync(m => m.MessageId == messageId);
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetMessagesByUserIdAsync(Guid userId)
//        {
//            return await _context.LiveChatMessages
//                .Where(m => m.UserId == userId)
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetMessagesByAdminIdAsync(Guid adminId)
//        {
//            return await _context.LiveChatMessages
//                .Where(m => m.AdminId == adminId)
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetAllMessagesAsync()
//        {
//            return await _context.LiveChatMessages
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task AddMessageAsync(LiveChatMessage message)
//        {
//            message.SentAt = DateTime.UtcNow;
//            await _context.LiveChatMessages.AddAsync(message);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateMessageAsync(LiveChatMessage message)
//        {
//            _context.LiveChatMessages.Update(message);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteMessageAsync(Guid messageId)
//        {
//            var message = await GetMessageByIdAsync(messageId);
//            if (message != null)
//            {
//                _context.LiveChatMessages.Remove(message);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public Task<IEnumerable<LiveChatMessage>> GetMessagesByClientIdAsync(string clientId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Interfaces;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;


//namespace brandiagaAPI2.Repositories
//{
//    public class LiveChatRepository : ILiveChatRepository
//    {
//        private readonly DbAbdeaeDotnet1Context _context;

//        public LiveChatRepository(DbAbdeaeDotnet1Context context)
//        {
//            _context = context;
//        }

//        public async Task<LiveChatMessage> GetMessageByIdAsync(Guid messageId)
//        {
//            return await _context.LiveChatMessages
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .FirstOrDefaultAsync(m => m.MessageId == messageId);
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetMessagesByUserIdAsync(Guid userId)
//        {
//            return await _context.LiveChatMessages
//                .Where(m => m.UserId == userId)
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetMessagesByAdminIdAsync(Guid adminId)
//        {
//            return await _context.LiveChatMessages
//                .Where(m => m.AdminId == adminId)
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<LiveChatMessage>> GetAllMessagesAsync()
//        {
//            return await _context.LiveChatMessages
//                .Include(m => m.User)
//                .Include(m => m.Admin)
//                .ToListAsync();
//        }

//        public async Task AddMessageAsync(LiveChatMessage message)
//        {
//            message.SentAt = DateTime.UtcNow;
//            await _context.LiveChatMessages.AddAsync(message);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateMessageAsync(LiveChatMessage message)
//        {
//            _context.LiveChatMessages.Update(message);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteMessageAsync(Guid messageId)
//        {
//            var message = await GetMessageByIdAsync(messageId);
//            if (message != null)
//            {
//                _context.LiveChatMessages.Remove(message);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public Task<IEnumerable<LiveChatMessage>> GetMessagesByClientIdAsync(string clientId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}