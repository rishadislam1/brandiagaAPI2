using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<CategoryResponseDto> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<CategoryResponseDto> UpdateCategoryAsync(Guid categoryId, CategoryUpdateDto categoryUpdateDto);
        Task DeleteCategoryAsync(Guid categoryId);
    }
}
