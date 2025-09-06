using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserResponseDto user);
    }

}
