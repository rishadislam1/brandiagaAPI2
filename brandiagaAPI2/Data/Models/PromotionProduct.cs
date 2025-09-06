using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class PromotionProduct
{
    public Guid PromotionProductId { get; set; }

    public Guid PromotionId { get; set; }

    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
