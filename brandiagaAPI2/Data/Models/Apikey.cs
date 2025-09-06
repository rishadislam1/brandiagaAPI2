using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Apikey
{
    public Guid ApikeyId { get; set; }

    public Guid UserId { get; set; }

    public string KeyValue { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
