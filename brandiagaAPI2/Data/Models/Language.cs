using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Language
{
    public Guid LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
