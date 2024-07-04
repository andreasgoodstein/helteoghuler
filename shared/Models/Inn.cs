using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Inn
{
    public Guid? Id { get; set; }

    public Chest Chest { get; set; }

    public IEnumerable<Hero> HeroRoster { get; set; }

    public string Name { get; set; }

    public IEnumerable<Hero> HeroRecruits { get; set; }
}
