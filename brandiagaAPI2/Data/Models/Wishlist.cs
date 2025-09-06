using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Wishlist
{
    public Guid WishlistId { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
