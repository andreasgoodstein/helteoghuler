using AutoMapper;
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

    public Adventure? Adventure { get; init; }

    public DateTime CreatedAt { get; init; }

    public Nullable<Guid> PlayerId { get; init; }

    public EventType Type => EventType.Adventure;

    public void ApplyToGameState(GameState gameState)
    {
        AdventureEventHelper.ApplyFallbackState(gameState);

        gameState.World.TotalAdventures += 1;
        gameState.Player.Inn.Chest.Gold += this.Adventure?.Gold ?? 0;
    }

    public void RemoveFromGameState(GameState gameState)
    {
        if (gameState?.World?.TotalAdventures > 0 && gameState?.Player?.Inn?.Chest?.Gold >= this.Adventure?.Gold)
        {
            gameState.World.TotalAdventures -= 1;
            gameState.Player.Inn.Chest.Gold -= this.Adventure?.Gold ?? 0;
        }
    }
}

public static class AdventureEventHelper
{
    private static Mapper _mapper;

    public static GameState MinimumGameState;

    static AdventureEventHelper()
    {
        MinimumGameState = new GameState
        {
            Player = new Player
            {
                Inn = new Inn
                {
                    Chest = new Chest
                    {
                        Gold = 0
                    }
                }
            },
            World = new World
            {
                TotalAdventures = 0
            }
        };

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<GameState, GameState>()));
    }

    public static void ApplyFallbackState(GameState gameState)
    {
        gameState = _mapper.Map(AdventureEventHelper.MinimumGameState, gameState);
    }
}