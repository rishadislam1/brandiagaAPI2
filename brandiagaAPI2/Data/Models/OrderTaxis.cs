using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class OrderTaxis
{
    public Guid OrderTaxId { get; set; }

    public Guid OrderId { get; set; }

    public Guid TaxId { get; set; }

    public decimal TaxAmount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Taxis Tax { get; set; } = null!;
}
