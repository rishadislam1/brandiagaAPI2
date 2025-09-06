using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data"));
            }

            try
            {
                var userResponse = await _userService.RegisterAsync(registerDto);
                return Ok(ResponseDTO<UserResponseDto>.Success(null, "Please Verify your email first"));
            }
            catch(Exception ex)
            {
                return BadRequest(ResponseDTO<Object>.Error(ex.Message));
            }
        }

        [HttpGet("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            try
            {
                await _userService.VerifyEmailAsync(token);

                // Redirect to the frontend with a query parameter indicating success
                return Redirect("https://brandiaga.com/?emailVerified=true");
            }
            catch (Exception ex)
            {
                // Redirect to the frontend with a query parameter indicating failure
                return Redirect($"https://brandiaga.com/?emailVerified=false&error={Uri.EscapeDataString(ex.Message)}");
            }
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var userResponse = await _userService.LoginAsync(loginDto);
                return Ok(ResponseDTO<UserResponseDto>.Success(userResponse, "Login Successfull"));
            }catch (Exception ex)
            {
                return BadRequest(ResponseDTO<Object>.Error(ex.Message));
            }
        }

        [HttpPost("subscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> Subscribe([FromBody] UserSubscribeDTO userSubscribeDto)
        {
            try
            {
                var usreResponse = await _userService.UserSubscribtionAsync(userSubscribeDto.Email);
                return Ok(ResponseDTO<string>.Success("","Successfully Subscribe"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<Object>.Error(ex.Message));
            }
        }

        [HttpGet("profile")]
        [Authorize(Policy = "SameUserOrAdmin")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(ResponseDTO<UserResponseDto>.Success(user, "Profile retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId != userId)
                {
                    return Forbid();
                }

                var user = await _userService.UpdateUserAsync(userId, userUpdateDto);
                return Ok(ResponseDTO<UserResponseDto>.Success(user, "User updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok(ResponseDTO<object>.Success(null, "User deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {

            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(ResponseDTO<IEnumerable<UserResponseDto>>.Success(users, "User retreived successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
           
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("customer/{userId:guid}/details")]
        public async Task<IActionResult> GetCustomerDetails(Guid userId)
        {
            try
            {
                var details = await _userService.GetCustomerDetailsAsync(userId);
                return Ok(ResponseDTO<CustomerDetailsDto>.Success(details, "Customer details retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("customer/{userId:guid}/order-history")]
        public async Task<IActionResult> GetCustomerOrderHistory(Guid userId)
        {
            try
            {
                var history = await _userService.GetCustomerOrderHistoryAsync(userId);
                return Ok(ResponseDTO<IEnumerable<CustomerOrderHistoryDto>>.Success(history, "Customer order history retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpPost("thirdPartyLoin")]    
        public async Task<IActionResult> ThirdPartyLogIn([FromBody] ThirdPartyUserLogInDTO thirdPartyUserLogIn)
        {
            try
            {
                var userResponse = await _userService.ThirdPartyLogInAsync(thirdPartyUserLogIn);
                return Ok(ResponseDTO<UserResponseDto>.Success(userResponse, "User Registered Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }


    }
}
