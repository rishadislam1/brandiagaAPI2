using brandiagaAPI2.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<Product> GetProductBySkuAsync(string sku);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
        Task AddProductImageAsync(ProductImage productImage);
        Task<IEnumerable<ProductImage>> GetProductImagesAsync(Guid productId);
        Task DeleteProductImagesAsync(Guid productId); // New method to delete images
        Task<decimal> GetAverageRatingAsync(Guid productId);
        Task<int> GetReviewCountAsync(Guid productId);

    }
}