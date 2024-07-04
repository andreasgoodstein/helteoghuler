using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HelteOgHulerServer.Interfaces;

public interface IEvent : IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public DateTime CreatedAt { get; init; }

    public EventType Type { get; }
}

public enum EventType
{
    Adventure = 0,
    NewPlayer = 1,
    RecruitHero = 2,
}