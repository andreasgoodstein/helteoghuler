using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Web.Data
{
    [BsonIgnoreExtraElements]
    public class Hero
    {
        public string Name { get; set; }
        public int BeerCount { get; set; }

        public DateTime TimeOfDeath { get; set; }
    }
}