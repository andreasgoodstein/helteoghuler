using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Player
{
    public Guid Id { get; set; }

    public Inn Inn { get; set; }

    public string Name { get; set; }

    public Nullable<DateTime> RestUntil { get; set; }

    // public Dictionary<PlayerObjectives, bool> ObjectivesCompleted { get; set; }
}

public class PlayerPublic
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string InnName { get; set; }

    public ulong TotalGoldEarned { get; set; }
}

// public enum PlayerObjectives
// {

// }
