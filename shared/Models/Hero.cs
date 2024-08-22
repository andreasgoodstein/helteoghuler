using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Hero
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ulong Price { get; set; }
    public HHAction[] ActionList { get; set; } = Actions.DefaultActions;
}
