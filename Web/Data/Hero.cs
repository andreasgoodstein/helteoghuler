using MongoDB.Bson;
using System;

namespace Web.Data
{
    public class Hero
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public int BeerCount { get; set; }

        public DateTime TimeOfDeath { get; set; }
    }
}