using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByIdAsync(Guid inventoryId);
        Task<Inventory> GetInventoryByProductIdAsync(Guid productId);
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(Guid inventoryId);
    }
}
