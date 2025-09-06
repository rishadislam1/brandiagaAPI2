using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using brandiagaAPI2.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace brandiagaAPI2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find user by email
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email");
            }
            else if ((bool)!user.IsActive)
            {
                throw new Exception("User is not active");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid Password");
            }

            // Get the user's role name
            var roleName = user.Role?.RoleName ?? "User";

            // Generate JWT token
            var token = GenerateJwtToken(user, roleName);

            // Return user details with token
            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = roleName,
                Token = token // Include the token in the response
            };
        }
        public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if(existingUser != null)
            {
                throw new Exception("Email Already Register");
            }

            // get the user role
            var roleName = registerDto.Role ?? "User";

            var userRole = await _userRepository.GetRoleByNameAsync(roleName);

            if (userRole == null)
            {
                throw new Exception("User role not found");
            }

            // create user
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                PhoneNumber = registerDto.PhoneNumber,
                RoleId = userRole.RoleId,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // save user to database
            await _userRepository.AddUserAsync(user);
            var token = GenerateVerificationToken();
            var verificationToken = new VerificationToken
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
            await _userRepository.AddVerificationTokenAsync(verificationToken);

            // Send verification email
            var verificationLink = $"{_configuration["AppUrl"]}/api/Users/verify-email?token={token}";
            await _emailService.SendEmailAsync(registerDto.Email, "Verify Your Email",
                $"Hi {registerDto.FirstName}, please verify your email by clicking this link: {verificationLink}");
            // return user details
            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = userRole.RoleName
            };
        }

        // verify email
        public async Task VerifyEmailAsync(string token)
        {
            var verificationToken = await _userRepository.GetVerificationTokenAsync(token);
            if (verificationToken == null)
                throw new Exception("Invalid verification token");
            if (verificationToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Verification token has expired");

            var user = verificationToken.User;
            user.IsActive = true;
            await _userRepository.UpdateAsync(user);

            // Send welcome email and SMS
            await _emailService.SendEmailAsync(user.Email, "Welcome to Brandiaga",
                $"Hi {user.FirstName}, your account has been verified successfully!");

        }

        public async Task<UserResponseDto> ThirdPartyLogInAsync(ThirdPartyUserLogInDTO thirdPartyUserLogInDTO)
        {
            var existinUser = await _userRepository.GetUserByEmailAsync(thirdPartyUserLogInDTO.Email);
            if (existinUser != null)
            {
                var roleName = existinUser.Role?.RoleName ?? "User";

                // Generate JWT token
                var token = GenerateJwtToken(existinUser, roleName);

                // Return user details with token
                return new UserResponseDto
                {
                    UserId = existinUser.UserId,
                    FirstName = existinUser.FirstName,
                    LastName = existinUser.LastName,
                    Email = existinUser.Email,
                    PhoneNumber = existinUser.PhoneNumber,
                    RoleName = roleName,
                    Token = token
                };
            }
            var userRole = await _userRepository.GetRoleByNameAsync("User");
          
            var user = new User
                {
                    FirstName = thirdPartyUserLogInDTO.Name,
                    LastName = "",
                    Email = thirdPartyUserLogInDTO.Email,
                    PasswordHash = "",
                    PhoneNumber = "",
                    RoleId = userRole.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // save user to database
                await _userRepository.AddUserAsync(user);
            var token1 = GenerateJwtToken(user, "User");
            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = userRole.RoleName,
                Token = token1
            };

        }

        public async Task<UserSubscribeDTO?> UserSubscribtionAsync(string email)
        {
            var user = await _userRepository.UserSubscription(email);

            return new UserSubscribeDTO
            {
                Email = user.Email
            };
        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            return new UserResponseDto
            {
                UserId = existingUser.UserId,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = existingUser.Email,
                PhoneNumber = existingUser.PhoneNumber,
                RoleName = existingUser.Role.RoleName,
                Address = existingUser.UserAddress,
                City = existingUser.City,
                Appartment = existingUser.Appartment,
                Country = existingUser.Country,
                State = existingUser.State,
                ZipCode = existingUser.ZipCode
            };
        }
        public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.UserAddress = userUpdateDto.Address;
            user.UpdatedAt = DateTime.UtcNow;
            user.City = userUpdateDto.City;
            user.Appartment = userUpdateDto.Appartment;
            user.Country = userUpdateDto.Country;
            user.State = userUpdateDto.State;
            user.ZipCode = userUpdateDto.ZipCode;

            await _userRepository.UpdateAsync(user);

            var updatedUser = await _userRepository.GetUserByIdAsync(userId);
            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = user.Role?.RoleName ?? "User",
                Address = user.UserAddress,
                City = user.City,
                Appartment = user.Appartment,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode
            };
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

           

            await _userRepository.DeleteAsync(userId);
        }


        public async Task<CustomerDetailsDto> GetCustomerDetailsAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Customer not found");
            }

            return new CustomerDetailsDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                RoleName = user.Role?.RoleName,
                TotalOrders = await _userRepository.GetTotalOrdersByUserIdAsync(userId),
                TotalSpent = await _userRepository.GetTotalSpentByUserIdAsync(userId)
            };
        }

        public async Task<IEnumerable<CustomerOrderHistoryDto>> GetCustomerOrderHistoryAsync(Guid userId)
        {
            var orders = await _userRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(order => new CustomerOrderHistoryDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status
            }).ToList();
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUserAsync();

            return users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                RoleName = u.Role?.RoleName ?? "User",
                Address = u.UserAddress

            }).ToList();
        }


        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = user.Role?.RoleName ?? "User",
                PasswordHash = user.PasswordHash // Include for social login
            };
        }
        // Services/UserService.cs
        public async Task<UserResponseDto> SocialLoginAsync(string email, string firstName, string lastName)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                // Create a new user for social login
                var userRole = await _userRepository.GetRoleByNameAsync("User");
                if (userRole == null)
                {
                    throw new Exception("User role not found");
                }

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = null, // No password for social login
                    PhoneNumber = "",
                    RoleId = userRole.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _userRepository.AddUserAsync(user);
            }
            else if (!user.IsActive)
            {
                throw new Exception("User is not active");
            }

            // Generate JWT token
            var roleName = user.Role?.RoleName ?? "User";
            var token = GenerateJwtToken(user, roleName);

            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = roleName,
                Token = token
            };
        }

        private string GenerateVerificationToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }


        private string GenerateJwtToken(User user, string roleName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


      
    }
}
