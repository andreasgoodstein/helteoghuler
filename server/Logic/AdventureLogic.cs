using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerServer.Logic;

public class AdventureLogic
{
    public bool CanPlayerGenerateAdventure()
    {
        return true;
    }

    public Adventure GenerateAdventure()
    {
        return new Adventure() { Gold = 1, Id = Guid.NewGuid() };
    }
}