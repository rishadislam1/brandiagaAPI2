using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class ProductImage
{
    public Guid ImageId { get; set; }

    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
