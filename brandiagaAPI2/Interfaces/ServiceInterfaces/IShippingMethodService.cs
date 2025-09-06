using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IShippingMethodService
    {
        Task<ShippingMethodResponseDto> CreateShippingMethodAsync(ShippingMethodCreateDto shippingMethodCreateDto);
        Task<ShippingMethodResponseDto> GetShippingMethodByIdAsync(Guid shippingMethodId);
        Task<IEnumerable<ShippingMethodResponseDto>> GetAllShippingMethodsAsync();
        Task<ShippingMethodResponseDto> UpdateShippingMethodAsync(Guid shippingMethodId, ShippingMethodUpdateDto shippingMethodUpdateDto);
        Task DeleteShippingMethodAsync(Guid shippingMethodId);
    }
}
