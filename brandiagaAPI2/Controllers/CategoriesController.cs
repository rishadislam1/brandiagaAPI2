using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var category = await _categoryService.CreateCategoryAsync(categoryCreateDto);
                return Ok(ResponseDTO<CategoryResponseDto>.Success(category, "Category created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                return Ok(ResponseDTO<CategoryResponseDto>.Success(category, "Category retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(ResponseDTO<IEnumerable<CategoryResponseDto>>.Success(categories, "Categories retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var category = await _categoryService.UpdateCategoryAsync(categoryId, categoryUpdateDto);
                return Ok(ResponseDTO<CategoryResponseDto>.Success(category, "Category updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return Ok(ResponseDTO<object>.Success(null, "Category deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
