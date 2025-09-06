namespace brandiagaAPI2.Dtos
{
    public class BannerRequestDto
    {
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageBase64 { get; set; } // Added for image uploads
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class BannerResponseDto
    {
        public Guid BannerId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}