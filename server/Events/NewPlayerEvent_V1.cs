using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HelteOgHulerServer.Events;

[BsonDiscriminator("NewPlayerEvent_V1")]
public class NewPlayerEvent_V1 : IEvent, IApplicable
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; }

    public DateTime CreatedAt { get; init; }

    public EventType Type => EventType.NewPlayer;

    public required Player Player { get; init; }

    public void ApplyToGameState(ref GameState gameState, Guid? playerId)
    {
        if (Player?.Id == null)
        {
            return;
        }

        gameState.PrivatePlayerDict.Add(Player.Id, Player);
        gameState.PublicPlayerDict.Add(Player.Id, new PlayerPublic
        {
            Id = Player.Id,
            InnName = Player.Inn.Name,
            Name = Player.Name,
            TotalGoldEarned = 0,
        });
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId)
    {
        if (Player?.Id == null)
        {
            return;
        }

        gameState.PrivatePlayerDict.Remove(Player.Id);
        gameState.PublicPlayerDict.Remove(Player.Id);
    }
}
