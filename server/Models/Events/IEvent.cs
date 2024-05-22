using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HelteOgHulerServer.Models.Interfaces;

public interface IEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    public EventType Type { get; }
}

public enum EventType
{
    Adventure = 0
}