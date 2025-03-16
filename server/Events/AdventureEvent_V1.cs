using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("AdventureEvent_V1")]
public class AdventureEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }
    public EventType Type => EventType.Adventure;
    public required Adventure Adventure { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid PlayerId { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        Adventure?.ApplyToGameState(ref gameState, PlayerId);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        Adventure?.RemoveFromGameState(ref gameState, PlayerId);
    }
}
