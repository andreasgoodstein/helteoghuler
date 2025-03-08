using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("UpgradeInnEvent_V1")]
public class UpgradeInnEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }
    public EventType Type => EventType.UpgradeInn;
    public required DateTime CreatedAt { get; init; }
    public required InnUpgrade Upgrade { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        Upgrade.ApplyToGameState(ref gameState);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        Upgrade.RemoveFromGameState(ref gameState);
    }
}
