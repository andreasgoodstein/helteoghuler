using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("RecruitHeroEvent_V1")]
public class RecruitHeroEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }
    public EventType Type => EventType.RecruitHero;
    public required DateTime CreatedAt { get; init; }
    public required Recruitment Recruitment { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        Recruitment.ApplyToGameState(ref gameState);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        Recruitment.RemoveFromGameState(ref gameState);
    }
}
