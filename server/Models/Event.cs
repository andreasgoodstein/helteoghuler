using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HelteOgHulerServer.Models;

public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public EventType Type { get; set; } = EventType.Adventure;
}

public enum EventType
{
    Adventure = 0
}