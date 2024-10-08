using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("UpgradeInnEvent_V1")]
public class UpgradeInnEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }
    public DateTime CreatedAt { get; init; }
    public EventType Type => EventType.UpgradeInn;
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
