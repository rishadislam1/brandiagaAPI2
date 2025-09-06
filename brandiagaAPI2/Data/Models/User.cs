using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public Guid RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? UserAddress { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? Appartment { get; set; }

    public virtual ICollection<Apikey> Apikeys { get; set; } = new List<Apikey>();

    public virtual ICollection<LiveChatMessage> LiveChatMessageAdmins { get; set; } = new List<LiveChatMessage>();

    public virtual ICollection<LiveChatMessage> LiveChatMessageUsers { get; set; } = new List<LiveChatMessage>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<VerificationToken> VerificationTokens { get; set; } = new List<VerificationToken>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
