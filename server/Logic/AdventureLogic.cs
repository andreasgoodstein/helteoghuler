using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerServer.Logic;

public class AdventureLogic
{
    const int REST_TIME_SEC = 10;

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
                RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC),
                Status = "Alas... Your party returns empty handed.",
            };
        }

        // Success
        var goldLiberated = (ulong)Random.Next(1, 10);

        return new Adventure
        {
            Gold = goldLiberated,
            RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC),
            Status = $"Forsooth! Your party was victorious and liberated {goldLiberated} gold."
        };
    }
}
