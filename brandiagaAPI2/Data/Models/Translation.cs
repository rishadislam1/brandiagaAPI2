using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Translation
{
    public Guid TranslationId { get; set; }

    public Guid LanguageId { get; set; }

    public string EntityType { get; set; } = null!;

    public Guid EntityId { get; set; }

    public string FieldName { get; set; } = null!;

    public string TranslatedValue { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
