using System;
using System.Collections.Generic;

namespace HelteOgHulerShared.Models;

public class Inn
{
    public Nullable<Guid> Id { get; set; }

    public Chest Chest { get; set; }

    public IEnumerable<Hero> HeroRoster { get; set; }

    public string Name { get; set; }
}
