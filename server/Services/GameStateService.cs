using HelteOgHulerServer.Models;
using HelteOgHulerShared.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HelteOgHulerServer.Services;

public class GameStateService
{
    private readonly IMongoCollection<GameState> _gameStateCollection;

    public GameStateService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _gameStateCollection = mongoDatabase.GetCollection<GameState>(
            databaseSettings.Value.GameStateCollectionName
        );
    }

    public Task CreateAsync(GameState state) => _gameStateCollection.InsertOneAsync(state);
}
