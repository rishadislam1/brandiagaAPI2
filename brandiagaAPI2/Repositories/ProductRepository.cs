using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public ProductRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews) 
                .ToListAsync();
        }


        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product> GetProductBySkuAsync(string sku)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Sku == sku);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductImageAsync(ProductImage productImage)
        {
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync(Guid productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
        }

        public async Task DeleteProductImagesAsync(Guid productId)
        {
            var images = await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
            if (images.Any())
            {
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<decimal> GetAverageRatingAsync(Guid productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (decimal?)r.Rating) ?? 0;
        }

        public async Task<int> GetReviewCountAsync(Guid productId)
        {
            return await _context.Reviews
                .CountAsync(r => r.ProductId == productId);
        }

    }
}