using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("CompleteObjectiveEvent_V1")]
public class CompleteObjectiveEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }
    public EventType Type => EventType.CompleteObjective;
    public required DateTime CreatedAt { get; init; }
    public required CompleteObjective CompleteObjective { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? playerId)
    {
        CompleteObjective.ApplyToGameState(ref gameState, playerId);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId)
    {
        CompleteObjective.RemoveFromGameState(ref gameState, playerId);
    }
}
