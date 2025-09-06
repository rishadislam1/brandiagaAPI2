using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Coupon
{
    public Guid CouponId { get; set; }

    public string Code { get; set; } = null!;

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public bool IsActive { get; set; }

    public int UsedCount { get; set; }

    public virtual ICollection<OrderCoupon> OrderCoupons { get; set; } = new List<OrderCoupon>();
}
