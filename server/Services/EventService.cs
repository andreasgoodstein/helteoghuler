using HelteOgHulerServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelteOgHulerServer.Services;

public class EventService
{
    private readonly IMongoCollection<Event> _eventsCollection;

    public EventService(
        IOptions<DatabaseSettings> eventStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            eventStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            eventStoreDatabaseSettings.Value.DatabaseName);

        _eventsCollection = mongoDatabase.GetCollection<Event>(
            eventStoreDatabaseSettings.Value.EventCollectionName);
    }

    public Task<List<Event>> GetAsync() =>
         _eventsCollection.Find(_ => true).ToListAsync();

    public Task<Event?> GetAsync(string id) =>
         _eventsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task<long> GetCount() => _eventsCollection.CountDocumentsAsync(new BsonDocument());

    public Task CreateAsync(Event newEvent) =>
         _eventsCollection.InsertOneAsync(newEvent);
}