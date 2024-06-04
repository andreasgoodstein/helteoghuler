using System;
using System.Collections.Generic;
using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public class Chest
{
    public Nullable<Guid> Id { get; set; }

    public ulong Gold { get; set; }

    public IEnumerable<IGear> GearPile { get; set; }
}
