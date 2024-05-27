using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelteOgHulerServer.Models;

[BsonDiscriminator("AdventureEvent")]
public class AdventureEvent : IEvent
{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string? Id { get; init; }

    public Adventure Adventure { get; init; }

    public DateTime CreatedAt { get; init; }

    public Guid PlayerId { get; init; }

    public EventType Type => EventType.Adventure;

    public void ApplyToGameState(GameState gameState)
    {
        gameState.World.TotalAdventures += 1;
        gameState.Player.Inn.Chest.Gold += this.Adventure.Gold;
    }

    public void RemoveFromGameState(GameState gameState)
    {
        gameState.World.TotalAdventures -= 1;
        gameState.Player.Inn.Chest.Gold -= this.Adventure.Gold;
    }
}
