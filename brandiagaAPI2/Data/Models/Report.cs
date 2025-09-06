using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Report
{
    public Guid ReportId { get; set; }

    public string ReportType { get; set; } = null!;

    public string? DataJson { get; set; }

    public DateTime GeneratedAt { get; set; }
}
