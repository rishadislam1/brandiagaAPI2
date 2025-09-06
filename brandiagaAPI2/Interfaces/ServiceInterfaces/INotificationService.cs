using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(Guid userId);
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto notificationDto);
        Task UpdateNotificationAsync(Guid notificationId, NotificationRequestDto notificationDto);
        Task DeleteNotificationAsync(Guid notificationId);
        Task SendEmailNotificationAsync(Guid userId, string subject, string body);
    }
}
