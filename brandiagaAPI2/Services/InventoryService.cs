using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<InventoryResponseDto> CreateInventoryAsync(InventoryCreateDto inventoryCreateDto)
        {
            // Check if product exists
            var product = await _productRepository.GetProductByIdAsync(inventoryCreateDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Check if inventory already exists for this product
            var existingInventory = await _inventoryRepository.GetInventoryByProductIdAsync(inventoryCreateDto.ProductId);
            if (existingInventory != null)
            {
                throw new Exception("Inventory already exists for this product");
            }

            var inventory = new Inventory
            {
                InventoryId = Guid.NewGuid(),
                ProductId = inventoryCreateDto.ProductId,
                Quantity = inventoryCreateDto.Quantity,
                LastUpdated = DateTime.UtcNow,
                
            };

            await _inventoryRepository.AddInventoryAsync(inventory);

            return new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = product.Name,
                Quantity = inventory.Quantity,
                CreatedAt = inventory.LastUpdated,
                UpdatedAt = inventory.LastUpdated
            };
        }

        public async Task<InventoryResponseDto> GetInventoryByIdAsync(Guid inventoryId)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found");
            }

            return new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name,
                Quantity = inventory.Quantity,
                CreatedAt = inventory.LastUpdated,
                UpdatedAt = inventory.LastUpdated
            };
        }

        public async Task<InventoryResponseDto> GetInventoryByProductIdAsync(Guid productId)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found for this product");
            }

            return new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name,
                Quantity = inventory.Quantity,
                CreatedAt = inventory.LastUpdated,
                UpdatedAt = inventory.LastUpdated
            };
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoriesAsync()
        {
            var inventories = await _inventoryRepository.GetAllInventoriesAsync();

            return inventories.Select(inventory => new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name,
                Quantity = inventory.Quantity,
                CreatedAt = inventory.LastUpdated,
                UpdatedAt = inventory.LastUpdated
            }).ToList();
        }

        public async Task<InventoryResponseDto> UpdateInventoryAsync(Guid inventoryId, InventoryUpdateDto inventoryUpdateDto)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found");
            }

            if (inventoryUpdateDto.Quantity.HasValue)
            {
                inventory.Quantity = inventoryUpdateDto.Quantity.Value;
            }

            inventory.LastUpdated = DateTime.UtcNow;

            await _inventoryRepository.UpdateInventoryAsync(inventory);

            return new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name,
                Quantity = inventory.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public async Task DeleteInventoryAsync(Guid inventoryId)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found");
            }

            await _inventoryRepository.DeleteInventoryAsync(inventoryId);
        }
    }
}
