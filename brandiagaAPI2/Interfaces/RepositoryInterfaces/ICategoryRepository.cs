using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid categoryId);
    }
}
