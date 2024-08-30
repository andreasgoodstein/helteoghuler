using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;


[BsonDiscriminator("RecruitHeroEvent_V1")]
public class RecruitHeroEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public DateTime CreatedAt { get; init; }

    public EventType Type => EventType.RecruitHero;

    public required Recruitment Recruitment { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        Recruitment.ApplyToGameState(ref gameState, null);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        Recruitment.RemoveFromGameState(ref gameState, null);
    }
}
