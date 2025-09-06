using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public Guid? ParentCategoryId { get; set; }
    }

    public class CategoryUpdateDto
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        public Guid? ParentCategoryId { get; set; }
    }

    public class CategoryResponseDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
