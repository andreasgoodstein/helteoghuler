using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Player
{
    public Nullable<Guid> Id { get; set; }

    public Inn Inn { get; set; }

    public string Name { get; set; }
}
