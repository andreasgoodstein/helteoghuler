using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerServer.Logic;

public class AdventureLogic
{
    private readonly Random Random;
    private readonly GameStateLogic _gameStateLogic;

    public AdventureLogic(GameStateLogic gameStateLogic)
    {
        Random = new Random();
        _gameStateLogic = gameStateLogic;
    }

    private bool CanPlayerAdventureForth(Guid playerId)
    {
        return (_gameStateLogic.Get()?.PrivatePlayerDict[playerId]?.RestUntil ?? DateTime.UtcNow) <= DateTime.UtcNow;
    }

    public Adventure GenerateAdventure(Guid playerId)
    {
        if (!CanPlayerAdventureForth(playerId))
        {
            throw new InvalidOperationException("Your party needs more rest, and cannot venture forth yet.");
        }

        // Failure
        if (Random.NextSingle() < .5)
        {
            return new Adventure
            {
                Gold = 0,
                RestUntil = DateTime.UtcNow.AddSeconds(10),
                Status = "Alas... Your party returns empty handed.",
            };
        }

        // Success
        return new Adventure
        {
            Gold = (ulong)Random.NextInt64(1, 10),
            RestUntil = DateTime.UtcNow.AddSeconds(10),
            Status = "Forsooth! Your party was victorious."
        };
    }
}
