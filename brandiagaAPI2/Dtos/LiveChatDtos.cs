using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Dtos
{
    public class LiveChatMessage
    {
        public Guid MessageId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? AdminId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime? SentAt { get; set; }
        public bool? IsRead { get; set; }
        public User? User { get; set; }
        public User? Admin { get; set; }
    }

    public class LiveChatMessageRequestDto { public Guid? UserId { get; set; } public Guid? AdminId { get; set; } public string ClientId { get; set; } public string Message { get; set; } public bool? IsRead { get; set; } }

    public class LiveChatMessageResponseDto { public Guid MessageId { get; set; } public Guid? UserId { get; set; } public Guid? AdminId { get; set; } public string ClientId { get; set; } public string Message { get; set; } public DateTime SentAt { get; set; } public bool IsRead { get; set; } }
}
