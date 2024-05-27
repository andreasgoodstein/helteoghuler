using System;

namespace HelteOgHulerShared.Interfaces;

public interface IGear
{
    Guid Id { get; set; }

    string Name { get; set; }
}
