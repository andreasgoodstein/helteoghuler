using HelteOgHulerServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelteOgHulerServer.Services;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UserCollectionName);
    }

    public Task<List<User>> GetAsync() =>
         _usersCollection.Find(_ => true).ToListAsync();

    // public Task<User> GetAsync(string id) =>
    //      _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // public Task<List<User>> GetAsyncAsc() =>
    //     _usersCollection.Find(_ => true).Sort(Builders<User>.Sort.Ascending("CreatedAt")).ToListAsync();

    public Task CreateAsync(User newEvent) =>
         _usersCollection.InsertOneAsync(newEvent);
}