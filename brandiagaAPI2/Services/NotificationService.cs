//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Dtos;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using brandiagaAPI2.Interfaces.ServiceInterfaces;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Services
//{
//    public class NotificationService : INotificationService
//    {
//        private readonly INotificationRepository _notificationRepository;
//        private readonly IConfiguration _configuration;

//        public NotificationService(INotificationRepository notificationRepository, IConfiguration configuration)
//        {
//            _notificationRepository = notificationRepository;
//            _configuration = configuration;
//        }

//        public async Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId)
//        {
//            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
//            if (notification == null) throw new Exception("Notification not found");

//            return new NotificationResponseDto
//            {
//                NotificationId = notification.NotificationId,
//                UserId = notification.UserId,
//                Type = notification.Type,
//                Message = notification.Message,
//                Status = notification.Status,
//                SentAt = notification.SentAt
//            };
//        }

//        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(Guid userId)
//        {
//            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
//            return notifications.Select(n => new NotificationResponseDto
//            {
//                NotificationId = n.NotificationId,
//                UserId = n.UserId,
//                Type = n.Type,
//                Message = n.Message,
//                Status = n.Status,
//                SentAt = n.SentAt
//            }).ToList();
//        }

//        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
//        {
//            var notifications = await _notificationRepository.GetAllNotificationsAsync();
//            return notifications.Select(n => new NotificationResponseDto
//            {
//                NotificationId = n.NotificationId,
//                UserId = n.UserId,
//                Type = n.Type,
//                Message = n.Message,
//                Status = n.Status,
//                SentAt = n.SentAt
//            }).ToList();
//        }

//        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto)
//        {
//            var notification = new Notification
//            {
//                NotificationId = Guid.NewGuid(),
//                UserId = notificationDto.UserId,
//                Type = notificationDto.Type,
//                Message = notificationDto.Message,
//                Status = notificationDto.Status,
//                SentAt = DateTime.UtcNow
//            };

//            await _notificationRepository.AddNotificationAsync(notification);

//            // Send email notification if type indicates email
//            if (notification.Type.ToLower() == "email")
//            {
//                await SendEmailNotificationAsync(notification.UserId, "New Notification", notification.Message);
//            }

//            return new NotificationResponseDto
//            {
//                NotificationId = notification.NotificationId,
//                UserId = notification.UserId,
//                Type = notification.Type,
//                Message = notification.Message,
//                Status = notification.Status,
//                SentAt = notification.SentAt
//            };
//        }

//        public async Task UpdateNotificationAsync(Guid notificationId, NotificationRequestDto notificationDto)
//        {
//            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
//            if (notification == null) throw new Exception("Notification not found");

//            notification.UserId = notificationDto.UserId;
//            notification.Type = notificationDto.Type;
//            notification.Message = notificationDto.Message;
//            notification.Status = notificationDto.Status;

//            await _notificationRepository.UpdateNotificationAsync(notification);
//        }

//        public async Task DeleteNotificationAsync(Guid notificationId)
//        {
//            await _notificationRepository.DeleteNotificationAsync(notificationId);
//        }

//        public async Task SendEmailNotificationAsync(Guid userId, string subject, string body)
//        {
//            var user = await _notificationRepository.GetNotificationByIdAsync(userId); // Assuming UserId can be used to fetch user email
//            if (user == null) throw new Exception("User not found");

//            var smtpSettings = _configuration.GetSection("SmtpSettings");
//            var smtpClient = new SmtpClient
//            {
//                Host = smtpSettings["Host"],
//                Port = int.Parse(smtpSettings["Port"]),
//                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
//                Credentials = new System.Net.NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
//            };

//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(smtpSettings["FromEmail"]),
//                Subject = subject,
//                Body = body,
//                IsBodyHtml = true
//            };
//            mailMessage.To.Add(user.User.Email); // Assuming User model has an Email property

//            await smtpClient.SendMailAsync(mailMessage);
//        }
//    }
//}
//using brandiagaAPI2.Data.Models;
//using brandiagaAPI2.Dtos;
//using brandiagaAPI2.Interfaces.RepositoryInterfaces;
//using brandiagaAPI2.Interfaces.ServiceInterfaces;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Services
//{
//    public class NotificationService : INotificationService
//    {
//        private readonly INotificationRepository _notificationRepository;
//        private readonly IConfiguration _configuration;

//        public NotificationService(INotificationRepository notificationRepository, IConfiguration configuration)
//        {
//            _notificationRepository = notificationRepository;
//            _configuration = configuration;
//        }

//        public async Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId)
//        {
//            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
//            if (notification == null) throw new Exception("Notification not found");

//            return new NotificationResponseDto
//            {
//                NotificationId = notification.NotificationId,
//                UserId = notification.UserId,
//                Type = notification.Type,
//                Message = notification.Message,
//                Status = notification.Status,
//                SentAt = notification.SentAt
//            };
//        }

//        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(Guid userId)
//        {
//            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
//            return notifications.Select(n => new NotificationResponseDto
//            {
//                NotificationId = n.NotificationId,
//                UserId = n.UserId,
//                Type = n.Type,
//                Message = n.Message,
//                Status = n.Status,
//                SentAt = n.SentAt
//            }).ToList();
//        }

//        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
//        {
//            var notifications = await _notificationRepository.GetAllNotificationsAsync();
//            return notifications.Select(n => new NotificationResponseDto
//            {
//                NotificationId = n.NotificationId,
//                UserId = n.UserId,
//                Type = n.Type,
//                Message = n.Message,
//                Status = n.Status,
//                SentAt = n.SentAt
//            }).ToList();
//        }

//        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto)
//        {
//            var notification = new Notification
//            {
//                NotificationId = Guid.NewGuid(),
//                UserId = notificationDto.UserId,
//                Type = notificationDto.Type,
//                Message = notificationDto.Message,
//                Status = notificationDto.Status,
//                SentAt = DateTime.UtcNow
//            };

//            await _notificationRepository.AddNotificationAsync(notification);

//            // Send email notification if type indicates email
//            if (notification.Type.ToLower() == "email")
//            {
//                await SendEmailNotificationAsync(notification.UserId, "New Notification", notification.Message);
//            }

//            return new NotificationResponseDto
//            {
//                NotificationId = notification.NotificationId,
//                UserId = notification.UserId,
//                Type = notification.Type,
//                Message = notification.Message,
//                Status = notification.Status,
//                SentAt = notification.SentAt
//            };
//        }

//        public async Task UpdateNotificationAsync(Guid notificationId, NotificationRequestDto notificationDto)
//        {
//            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
//            if (notification == null) throw new Exception("Notification not found");

//            notification.UserId = notificationDto.UserId;
//            notification.Type = notificationDto.Type;
//            notification.Message = notificationDto.Message;
//            notification.Status = notificationDto.Status;

//            await _notificationRepository.UpdateNotificationAsync(notification);
//        }

//        public async Task DeleteNotificationAsync(Guid notificationId)
//        {
//            await _notificationRepository.DeleteNotificationAsync(notificationId);
//        }

//        public async Task SendEmailNotificationAsync(Guid userId, string subject, string body)
//        {
//            var user = await _notificationRepository.GetNotificationByIdAsync(userId); // Assuming UserId can be used to fetch user email
//            if (user == null) throw new Exception("User not found");

//            var smtpSettings = _configuration.GetSection("SmtpSettings");
//            var smtpClient = new SmtpClient
//            {
//                Host = smtpSettings["Host"],
//                Port = int.Parse(smtpSettings["Port"]),
//                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
//                Credentials = new System.Net.NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
//            };

//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(smtpSettings["FromEmail"]),
//                Subject = subject,
//                Body = body,
//                IsBodyHtml = true
//            };
//            mailMessage.To.Add(user.User.Email); // Assuming User model has an Email property

//            await smtpClient.SendMailAsync(mailMessage);
//        }
//    }
//}