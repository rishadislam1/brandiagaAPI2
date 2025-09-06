using brandiagaAPI2.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<ProductResponseDto> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto> UpdateProductAsync(Guid productId, ProductUpdateDto productUpdateDto);
        Task DeleteProductAsync(Guid productId);
    }
}