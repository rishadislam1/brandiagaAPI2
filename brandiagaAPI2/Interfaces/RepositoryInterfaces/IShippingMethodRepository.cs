using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IShippingMethodRepository
    {
        Task<ShippingMethod> GetShippingMethodByIdAsync(Guid shippingMethodId);
        Task<IEnumerable<ShippingMethod>> GetAllShippingMethodsAsync();
        Task AddShippingMethodAsync(ShippingMethod shippingMethod);
        Task UpdateShippingMethodAsync(ShippingMethod shippingMethod);
        Task DeleteShippingMethodAsync(Guid shippingMethodId);
    }
}
