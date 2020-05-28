using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Web.Data;

namespace Web.Storage
{
    public static class HighscoreStore
    {
        private const string DATABASE_KEY = "heroku_8l4ln147";

        private const string HIGHSCORE_KEY = "highscore";

        private static IMongoCollection<Hero> GetHeroCollection()
        {
            string databaseUrl = Environment.GetEnvironmentVariable("MONGODB_URI");

            var client = new MongoClient($"{databaseUrl}?retryWrites=false");

            var database = client.GetDatabase(DATABASE_KEY);

            return database.GetCollection<Hero>(HIGHSCORE_KEY);
        }

        public static List<Hero> GetHighscore()
        {
            var collection = GetHeroCollection();

            var heroList = collection.Find(doc => true);

            return heroList.ToList();
        }

        public static void AddHeroToHighscore(Hero hero)
        {
            var collection = GetHeroCollection();
            collection.InsertOne(hero);
        }

        public static void RemoveHeroFromHighscore(Hero hero)
        {
            var collection = GetHeroCollection();
            collection.DeleteOne(oldHero => oldHero.Id == hero.Id);
        }
    }
}