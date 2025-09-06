using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public CategoryRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
