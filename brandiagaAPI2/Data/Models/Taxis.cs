using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Taxis
{
    public Guid TaxId { get; set; }

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public decimal Rate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<OrderTaxis> OrderTaxes { get; set; } = new List<OrderTaxis>();
}
