using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;


[BsonDiscriminator("NewPlayerEvent")]
public class NewPlayerEvent : IEvent
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public EventType Type => EventType.NewPlayer;

    public Player? Player { get; init; }

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
            TotalTribute = 0,
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