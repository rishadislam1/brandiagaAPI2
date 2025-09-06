using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class OrderCoupon
{
    public Guid OrderCouponId { get; set; }

    public Guid OrderId { get; set; }

    public Guid CouponId { get; set; }

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
