using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Inn
{
    public Chest Chest { get; set; }

    public Dictionary<string, Hero> HeroRecruits { get; set; }

    public Dictionary<string, Hero> HeroRoster { get; set; }

    public string Name { get; set; }
}
