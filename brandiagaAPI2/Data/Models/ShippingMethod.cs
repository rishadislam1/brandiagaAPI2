using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class ShippingMethod
{
    public Guid ShippingMethodId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<OrderShipping> OrderShippings { get; set; } = new List<OrderShipping>();
}
