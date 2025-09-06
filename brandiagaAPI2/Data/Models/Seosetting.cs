using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Seosetting
{
    public Guid Seoid { get; set; }

    public string PageType { get; set; } = null!;

    public Guid PageId { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }
}
