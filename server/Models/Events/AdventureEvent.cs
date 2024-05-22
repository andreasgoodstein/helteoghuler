using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HelteOgHulerServer.Models.Interfaces;

namespace HelteOgHulerServer.Models;

public class AdventureEvent : IEvent
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    public ulong Gold { get; init; }

    public string PlayerId { get; init; } = "Unknown";

    public EventType Type => EventType.Adventure;
}