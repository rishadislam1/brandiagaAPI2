using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Inventory
{
    public Guid InventoryId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual Product Product { get; set; } = null!;
}
