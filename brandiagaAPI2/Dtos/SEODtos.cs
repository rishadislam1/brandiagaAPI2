using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Dtos
{
    public class SEORequestDto
    {
        public string PageType { get; set; } = null!;
        public Guid PageId { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public class SEOResponseDto
    {
        public Guid Seoid { get; set; }
        public string PageType { get; set; } = null!;
        public Guid PageId { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
