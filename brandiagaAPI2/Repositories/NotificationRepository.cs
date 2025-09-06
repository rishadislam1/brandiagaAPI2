//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Repositories
//{
//    public class NotificationRepository : INotificationRepository
//    {
//        private readonly DbAbdeaeDotnet1Context _context;

//        public NotificationRepository(DbAbdeaeDotnet1Context context)
//        {
//            _context = context;
//        }

//        public async Task<Notification> GetNotificationByIdAsync(Guid notificationId)
//        {
//            return await _context.Notifications
//                .Include(n => n.User)
//                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
//        }

//        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
//        {
//            return await _context.Notifications
//                .Where(n => n.UserId == userId)
//                .Include(n => n.User)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
//        {
//            return await _context.Notifications
//                .Include(n => n.User)
//                .ToListAsync();
//        }

//        public async Task AddNotificationAsync(Notification notification)
//        {
//            notification.SentAt = DateTime.UtcNow;
//            await _context.Notifications.AddAsync(notification);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateNotificationAsync(Notification notification)
//        {
//            _context.Notifications.Update(notification);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteNotificationAsync(Guid notificationId)
//        {
//            var notification = await GetNotificationByIdAsync(notificationId);
//            if (notification != null)
//            {
//                _context.Notifications.Remove(notification);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}
//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Repositories
//{
//    public class NotificationRepository : INotificationRepository
//    {
//        private readonly DbAbdeaeDotnet1Context _context;

//        public NotificationRepository(DbAbdeaeDotnet1Context context)
//        {
//            _context = context;
//        }

//        public async Task<Notification> GetNotificationByIdAsync(Guid notificationId)
//        {
//            return await _context.Notifications
//                .Include(n => n.User)
//                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
//        }

//        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
//        {
//            return await _context.Notifications
//                .Where(n => n.UserId == userId)
//                .Include(n => n.User)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
//        {
//            return await _context.Notifications
//                .Include(n => n.User)
//                .ToListAsync();
//        }

//        public async Task AddNotificationAsync(Notification notification)
//        {
//            notification.SentAt = DateTime.UtcNow;
//            await _context.Notifications.AddAsync(notification);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateNotificationAsync(Notification notification)
//        {
//            _context.Notifications.Update(notification);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteNotificationAsync(Guid notificationId)
//        {
//            var notification = await GetNotificationByIdAsync(notificationId);
//            if (notification != null)
//            {
//                _context.Notifications.Remove(notification);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}