using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Interfaces;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Models;

[BsonDiscriminator("AdventureEvent")]
public class AdventureEvent : IEvent, IApplicable
{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; init; }

    public Adventure? Adventure { get; init; }

    public DateTime CreatedAt { get; init; }

    public Nullable<Guid> PlayerId { get; init; }

    public EventType Type => EventType.Adventure;

    public void ApplyToGameState(ref GameState gameState)
    {
        if (Adventure != null)
        {
            Adventure.ApplyToGameState(ref gameState);
        }
    }

    public void RemoveFromGameState(ref GameState gameState)
    {
        if (Adventure != null)
        {
            Adventure.RemoveFromGameState(ref gameState);
        }
    }
}
