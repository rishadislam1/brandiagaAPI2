using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtService jwtService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/google/callback" };
            _logger.LogInformation("Initiating Google OAuth login");
            return Challenge(properties, "Google");
        }

        [HttpGet("facebook")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/facebook/callback" };
            _logger.LogInformation("Initiating Facebook OAuth login");
            return Challenge(properties, "Facebook");
        }

        [HttpGet("twitter")]
        public IActionResult TwitterLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/twitter/callback" };
            _logger.LogInformation("Initiating Twitter OAuth login");
            return Challenge(properties, "Twitter");
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogError("Google authentication failed: {Error}", result.Failure?.Message);
                return Redirect($"/Error?message={Uri.EscapeDataString(result.Failure?.Message ?? "Google authentication failed")}");
            }

            return await HandleCallback(result.Principal, "Google");
        }

        [HttpGet("facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogError("Facebook authentication failed: {Error}", result.Failure?.Message);
                return Redirect($"/Error?message={Uri.EscapeDataString(result.Failure?.Message ?? "Facebook authentication failed")}");
            }

            return await HandleCallback(result.Principal, "Facebook");
        }

        [HttpGet("twitter/callback")]
        public async Task<IActionResult> TwitterCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogError("Twitter authentication failed: {Error}", result.Failure?.Message);
                return Redirect($"/Error?message={Uri.EscapeDataString(result.Failure?.Message ?? "Twitter authentication failed")}");
            }

            return await HandleCallback(result.Principal, "Twitter");
        }

        private async Task<IActionResult> HandleCallback(ClaimsPrincipal principal, string provider)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("{Provider} did not provide an email.", provider);
                return Redirect($"/Error?message={Uri.EscapeDataString($"Email not provided by {provider}")}");
            }

            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    var registerDto = new RegisterDto
                    {
                        FirstName = name?.Split(' ')[0] ?? "User",
                        LastName = name?.Split(' ').Length > 1 ? name.Split(' ')[1] : "",
                        Email = email,
                        Password = Guid.NewGuid().ToString(),
                        PhoneNumber = ""
                    };

                    user = await _userService.RegisterAsync(registerDto);
                    _logger.LogInformation("Registered new user via {Provider}: {Email}", provider, email);
                }

                var token = _jwtService.GenerateToken(user);
                _logger.LogInformation("Generated JWT for user: {Email}", email);

                var frontendBaseUrl = _configuration["FrontendBaseUrl"] ?? "http://localhost:5173";
                var redirectUrl = $"{frontendBaseUrl}/auth/callback?token={token}&firstName={user.FirstName}&role=User";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {Provider} callback for email: {Email}", provider, email);
                return Redirect($"/Error?message={Uri.EscapeDataString(ex.Message)}");
            }
        }
    }
}