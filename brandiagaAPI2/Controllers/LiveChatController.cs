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
//    public class LiveChatController : ControllerBase
//    {
//        private readonly ILiveChatService _liveChatService;

//        public LiveChatController(ILiveChatService liveChatService)
//        {
//            _liveChatService = liveChatService;
//        }

//        [Authorize]
//        [HttpGet("{messageId:guid}")]
//        public async Task<IActionResult> GetMessageById(Guid messageId)
//        {
//            try
//            {
//                var message = await _liveChatService.GetMessageByIdAsync(messageId);
//                return Ok(ResponseDTO<LiveChatMessageResponseDto>.Success(message, "Message retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [HttpGet("user/{userId:guid}")]
//        public async Task<IActionResult> GetMessagesByUserId(Guid userId)
//        {
//            try
//            {
//                var messages = await _liveChatService.GetMessagesByUserIdAsync(userId);
//                return Ok(ResponseDTO<IEnumerable<LiveChatMessageResponseDto>>.Success(messages, "Messages retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("admin/{adminId:guid}")]
//        public async Task<IActionResult> GetMessagesByAdminId(Guid adminId)
//        {
//            try
//            {
//                var messages = await _liveChatService.GetMessagesByAdminIdAsync(adminId);
//                return Ok(ResponseDTO<IEnumerable<LiveChatMessageResponseDto>>.Success(messages, "Messages retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet]
//        public async Task<IActionResult> GetAllMessages()
//        {
//            try
//            {
//                var messages = await _liveChatService.GetAllMessagesAsync();
//                return Ok(ResponseDTO<IEnumerable<LiveChatMessageResponseDto>>.Success(messages, "All messages retrieved successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> SendMessage([FromBody] LiveChatMessageRequestDto messageDto)
//        {
//            try
//            {
//                if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
//                {
//                    return BadRequest(ResponseDTO<object>.Error("Message data is required."));
//                }
//                var message = await _liveChatService.SendMessageAsync(messageDto);
//                return Ok(ResponseDTO<LiveChatMessageResponseDto>.Success(message, "Message sent successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpPut("{messageId:guid}")]
//        public async Task<IActionResult> UpdateMessage(Guid messageId, [FromBody] LiveChatMessageRequestDto messageDto)
//        {
//            try
//            {
//                if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
//                {
//                    return BadRequest(ResponseDTO<object>.Error("Message data is required."));
//                }
//                await _liveChatService.UpdateMessageAsync(messageId, messageDto);
//                return Ok(ResponseDTO<object>.Success(null, "Message updated successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{messageId:guid}")]
//        public async Task<IActionResult> DeleteMessage(Guid messageId)
//        {
//            try
//            {
//                await _liveChatService.DeleteMessageAsync(messageId);
//                return Ok(ResponseDTO<object>.Success(null, "Message deleted successfully."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ResponseDTO<object>.Error(ex.Message));
//            }
//        }
//    }
//}