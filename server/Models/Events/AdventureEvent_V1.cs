using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HelteOgHulerServer.Models;

[BsonDiscriminator("AdventureEvent_V1")]
public class AdventureEvent_V1 : IEvent, IApplicable
{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public Adventure Adventure { get; init; }

    public DateTime CreatedAt { get; init; }

    public Guid? PlayerId { get; init; }

    public EventType Type => EventType.Adventure;

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        if (Adventure != null)
        {
            Adventure.ApplyToGameState(ref gameState, PlayerId);
        }
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        if (Adventure != null)
        {
            Adventure.RemoveFromGameState(ref gameState, PlayerId);
        }
    }
}