using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HelteOgHulerServer.Models.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Models;

public class AdventureEvent : IEvent
{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; set; }

    public Guid PlayerId { get; init; }

    public Adventure Adventure { get; init; }

    public EventType Type => EventType.Adventure;
}