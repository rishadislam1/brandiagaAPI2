using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            // Validate ParentCategoryId if provided
            if (categoryCreateDto.ParentCategoryId.HasValue)
            {
                var parentCategory = await _categoryRepository.GetCategoryByIdAsync(categoryCreateDto.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    throw new Exception("Parent category not found");
                }
            }

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = categoryCreateDto.Name,
                ParentCategoryId = categoryCreateDto.ParentCategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddCategoryAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories.Select(category => new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            }).ToList();
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            // Validate ParentCategoryId if provided
            if (categoryUpdateDto.ParentCategoryId.HasValue)
            {
                var parentCategory = await _categoryRepository.GetCategoryByIdAsync(categoryUpdateDto.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    throw new Exception("Parent category not found");
                }
                category.ParentCategoryId = categoryUpdateDto.ParentCategoryId;
            }

            if (!string.IsNullOrEmpty(categoryUpdateDto.Name))
            {
                category.Name = categoryUpdateDto.Name;
            }

            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.UpdateCategoryAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
    }
}
