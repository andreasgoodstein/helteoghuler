using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Interfaces;

public interface IEvent
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public EventType Type { get; }

    public void ApplyToGameState(GameState gameState);

    public void RemoveFromGameState(GameState gameState);
}

public enum EventType
{
    Adventure = 0
}