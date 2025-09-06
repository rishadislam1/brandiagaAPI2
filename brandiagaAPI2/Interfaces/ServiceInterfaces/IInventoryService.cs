using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IInventoryService
    {
        Task<InventoryResponseDto> CreateInventoryAsync(InventoryCreateDto inventoryCreateDto);
        Task<InventoryResponseDto> GetInventoryByIdAsync(Guid inventoryId);
        Task<InventoryResponseDto> GetInventoryByProductIdAsync(Guid productId);
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoriesAsync();
        Task<InventoryResponseDto> UpdateInventoryAsync(Guid inventoryId, InventoryUpdateDto inventoryUpdateDto);
        Task DeleteInventoryAsync(Guid inventoryId);
    }
}
