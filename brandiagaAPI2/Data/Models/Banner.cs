using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Banner
{
    public Guid BannerId { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
