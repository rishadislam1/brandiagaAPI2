//using brandiagaAPI2.Dtos;
//using brandiagaAPI2.Interfaces.ServiceInterfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;

//namespace brandiagaAPI2.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly INotificationService _notificationService;

//        public NotificationController(INotificationService notificationService)
//        {
//            _notificationService = notificationService;
//        }

//        [Authorize]
//        [HttpGet("{notificationId:guid}")]
//        public async Task<IActionResult> GetNotificationById(Guid notificationId)
//        {
//            try
//            {
//                var notification = await _notificationService.GetNotificationByIdAsync(notificationId);
//                return Ok(ResponseDTO<NotificationResponseDto>.Success(notification, "Notification retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize]
//        [HttpGet("user/{userId:guid}")]
//        public async Task<IActionResult> GetNotificationsByUserId(Guid userId)
//        {
//            try
//            {
//                var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
//                return Ok(ResponseDTO<IEnumerable<NotificationResponseDto>>.Success(notifications, "Notifications retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet]
//        public async Task<IActionResult> GetAllNotifications()
//        {
//            try
//            {
//                var notifications = await _notificationService.GetAllNotificationsAsync();
//                return Ok(ResponseDTO<IEnumerable<NotificationResponseDto>>.Success(notifications, "All notifications retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPost]
//        public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestDto notificationDto)
//        {
//            try
//            {
//                if (notificationDto == null)
//                {
//                    return BadRequest(ResponseDTO<object>.Error("Notification data is required."));
//                }
//                var notification = await _notificationService.CreateNotificationAsync(notificationDto);
//                return Ok(ResponseDTO<NotificationResponseDto>.Success(notification, "Notification created successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{notificationId:guid}")]
//        public async Task<IActionResult> DeleteNotification(Guid notificationId)
//        {
//            try
//            {
//                await _notificationService.DeleteNotificationAsync(notificationId);
//                return Ok(ResponseDTO<object>.Success(null, "Notification deleted successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }
//    }
//}