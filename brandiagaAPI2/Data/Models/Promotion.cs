using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Promotion
{
    public Guid PromotionId { get; set; }

    public string Name { get; set; } = null!;

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
}
