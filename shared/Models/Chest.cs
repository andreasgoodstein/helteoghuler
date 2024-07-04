using HelteOgHulerShared.Interfaces;
using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Chest
{
    public ulong Gold { get; set; }

    public IEnumerable<IGear> GearPile { get; set; }
}
