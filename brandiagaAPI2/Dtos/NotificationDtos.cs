namespace brandiagaAPI2.Dtos
{
    public class NotificationRequestDto
    {
        public Guid UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class NotificationResponseDto
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}
