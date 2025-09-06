using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserResponseDto> LoginAsync (LoginDto loginDto);
        Task<UserResponseDto> GetUserByIdAsync(Guid userId);
        Task<UserResponseDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto);
        Task DeleteUserAsync(Guid userId);
        Task<CustomerDetailsDto> GetCustomerDetailsAsync(Guid userId);
        Task<IEnumerable<CustomerOrderHistoryDto>> GetCustomerOrderHistoryAsync(Guid userId);
        Task<UserResponseDto> GetUserByEmailAsync(string email);
        Task<UserResponseDto> SocialLoginAsync(string email, string firstName, string lastName);
        Task<UserSubscribeDTO> UserSubscribtionAsync(string email);
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> ThirdPartyLogInAsync(ThirdPartyUserLogInDTO thirdPartyUserLogInDTO);
        Task VerifyEmailAsync(string token);
    }
}
