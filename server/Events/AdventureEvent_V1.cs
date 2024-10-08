using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("AdventureEvent_V1")]
public class AdventureEvent_V1 : IEvent, IApplicable
{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public required Adventure Adventure { get; init; }

    public DateTime CreatedAt { get; init; }

    public Guid PlayerId { get; init; }

    public EventType Type => EventType.Adventure;

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        Adventure?.ApplyToGameState(ref gameState, PlayerId);

        ApplyPendingInnUpgrade(ref gameState);
    }

    private void ApplyPendingInnUpgrade(ref GameState gameState)
    {
        var inn = gameState.GetPlayer(PlayerId).Inn;

        var pendingUpgrade = inn.PendingUpgrade;
        if (pendingUpgrade == null) { return; }

        inn.AvailableUpgrades.Remove((InnUpgradeName)pendingUpgrade);
        inn.BuiltUpgrades.Add((InnUpgradeName)pendingUpgrade);
        inn.AvailableUpgrades.AddRange(InnUpgrades.TechTree[(InnUpgradeName)pendingUpgrade]);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        Adventure?.RemoveFromGameState(ref gameState, PlayerId);

        RemovePendingInnUpgrade(ref gameState);
    }

    private void RemovePendingInnUpgrade(ref GameState gameState)
    {
        var inn = gameState.GetPlayer(PlayerId).Inn;

        if (inn.BuiltUpgrades.Count < 1) { return; }

        // ATT: This might be a bit brittle. However the events should be applied/removed in sequence.
        var pendingUpgrade = inn.BuiltUpgrades.Last();

        foreach (var upgrade in InnUpgrades.TechTree[pendingUpgrade])
        {
            inn.AvailableUpgrades.Remove(upgrade);
        }

        inn.AvailableUpgrades.Add(pendingUpgrade);
        inn.BuiltUpgrades.Remove(pendingUpgrade);
    }
}
