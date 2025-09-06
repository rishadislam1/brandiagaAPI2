using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid OrderId { get; set; }

    public Guid GatewayId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string TransactionStatus { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public virtual PaymentGateway Gateway { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
