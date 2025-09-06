using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class PaymentGateway
{
    public Guid GatewayId { get; set; }

    public string Name { get; set; } = null!;

    public string? ConfigJson { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
