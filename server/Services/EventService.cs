using HelteOgHulerServer.Models;
using HelteOgHulerServer.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelteOgHulerServer.Services;

public class EventService
{
    private readonly IMongoCollection<IEvent> _eventsCollection;

    public EventService(
        IOptions<DatabaseSettings> eventStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            eventStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            eventStoreDatabaseSettings.Value.DatabaseName);

        _eventsCollection = mongoDatabase.GetCollection<IEvent>(
            eventStoreDatabaseSettings.Value.EventCollectionName);
    }

    public Task<List<IEvent>> GetAsync() =>
         _eventsCollection.Find(_ => true).ToListAsync();

    public Task<IEvent> GetAsync(string id) =>
         _eventsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task<long> GetCount() => _eventsCollection.CountDocumentsAsync(new BsonDocument());

    public Task CreateAsync(IEvent newEvent) =>
         _eventsCollection.InsertOneAsync(newEvent);
}