using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class ShippingMethodService : IShippingMethodService
    {
        private readonly IShippingMethodRepository _shippingMethodRepository;

        public ShippingMethodService(IShippingMethodRepository shippingMethodRepository)
        {
            _shippingMethodRepository = shippingMethodRepository;
        }

        public async Task<ShippingMethodResponseDto> CreateShippingMethodAsync(ShippingMethodCreateDto shippingMethodCreateDto)
        {
            // Check if a shipping method with the same name already exists
            var existingMethod = await _shippingMethodRepository.GetAllShippingMethodsAsync();
            if (existingMethod.Any(sm => sm.Name.Equals(shippingMethodCreateDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Shipping method with this name already exists");
            }

            var shippingMethod = new ShippingMethod
            {
                ShippingMethodId = Guid.NewGuid(),
                Name = shippingMethodCreateDto.Name,
                Cost = shippingMethodCreateDto.Cost,
                IsActive = shippingMethodCreateDto.IsActive
            };

            await _shippingMethodRepository.AddShippingMethodAsync(shippingMethod);

            return new ShippingMethodResponseDto
            {
                ShippingMethodId = shippingMethod.ShippingMethodId,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
                IsActive = shippingMethod.IsActive
            };
        }

        public async Task<ShippingMethodResponseDto> GetShippingMethodByIdAsync(Guid shippingMethodId)
        {
            var shippingMethod = await _shippingMethodRepository.GetShippingMethodByIdAsync(shippingMethodId);
            if (shippingMethod == null)
            {
                throw new Exception("Shipping method not found");
            }

            return new ShippingMethodResponseDto
            {
                ShippingMethodId = shippingMethod.ShippingMethodId,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
                IsActive = shippingMethod.IsActive
            };
        }

        public async Task<IEnumerable<ShippingMethodResponseDto>> GetAllShippingMethodsAsync()
        {
            var shippingMethods = await _shippingMethodRepository.GetAllShippingMethodsAsync();
            return shippingMethods.Select(sm => new ShippingMethodResponseDto
            {
                ShippingMethodId = sm.ShippingMethodId,
                Name = sm.Name,
                Cost = sm.Cost,
                IsActive = sm.IsActive
            }).ToList();
        }

        public async Task<ShippingMethodResponseDto> UpdateShippingMethodAsync(Guid shippingMethodId, ShippingMethodUpdateDto shippingMethodUpdateDto)
        {
            var shippingMethod = await _shippingMethodRepository.GetShippingMethodByIdAsync(shippingMethodId);
            if (shippingMethod == null)
            {
                throw new Exception("Shipping method not found");
            }

            if (!string.IsNullOrEmpty(shippingMethodUpdateDto.Name))
            {
                var existingMethods = await _shippingMethodRepository.GetAllShippingMethodsAsync();
                if (existingMethods.Any(sm => sm.Name.Equals(shippingMethodUpdateDto.Name, StringComparison.OrdinalIgnoreCase) && sm.ShippingMethodId != shippingMethodId))
                {
                    throw new Exception("Shipping method with this name already exists");
                }
                shippingMethod.Name = shippingMethodUpdateDto.Name;
            }

            if (shippingMethodUpdateDto.Cost.HasValue)
            {
                shippingMethod.Cost = shippingMethodUpdateDto.Cost.Value;
            }

            if (shippingMethodUpdateDto.IsActive.HasValue)
            {
                shippingMethod.IsActive = shippingMethodUpdateDto.IsActive.Value;
            }

            await _shippingMethodRepository.UpdateShippingMethodAsync(shippingMethod);

            return new ShippingMethodResponseDto
            {
                ShippingMethodId = shippingMethod.ShippingMethodId,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
                IsActive = shippingMethod.IsActive
            };
        }

        public async Task DeleteShippingMethodAsync(Guid shippingMethodId)
        {
            var shippingMethod = await _shippingMethodRepository.GetShippingMethodByIdAsync(shippingMethodId);
            if (shippingMethod == null)
            {
                throw new Exception("Shipping method not found");
            }

            // Note: Not checking OrderShippings dependency since we're not using other models
            await _shippingMethodRepository.DeleteShippingMethodAsync(shippingMethodId);
        }
    }
}
