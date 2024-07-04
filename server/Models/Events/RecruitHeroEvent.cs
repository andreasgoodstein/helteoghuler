using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;


[BsonDiscriminator("RecruitHeroEvent_V1")]
public class RecruitHeroEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public DateTime CreatedAt { get; init; }

    public EventType Type => EventType.RecruitHero;

    public Recruitment Recruitment { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? playerId)
    {
        Recruitment.ApplyToGameState(ref gameState, playerId);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId)
    {
        Recruitment.RemoveFromGameState(ref gameState, playerId);
    }
}
