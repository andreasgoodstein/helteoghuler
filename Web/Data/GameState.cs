using System;
using System.Collections.Generic;
using Web.Data;

namespace Web
{
    public class GameState
    {
        private const int START_HEALTH = 3;

        public string HeroName { get; set; }

        public int BeerCount { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Health { get; set; } = START_HEALTH;
        public int MaxHealth { get; set; } = START_HEALTH;

        public Data.GameLocation Location { get; set; } = Data.GameLocation.Værtshuset;

        public List<string> Log { get; set; } = new List<string>();

        [NonSerialized]
        public List<Hero> Highscore = new List<Hero>();

        public void UpdateLog(string entry)
        {
            Log.Add($"{DateTime.UtcNow.ToLongTimeString()}: {entry}");
        }
    }
}