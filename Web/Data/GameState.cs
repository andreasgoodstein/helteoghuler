using System;
using System.Collections.Generic;

namespace Web
{
    public class GameState
    {
        private const int START_HEALTH = 3;

        public int BeerCount { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Health { get; set; } = START_HEALTH;
        public int MaxHealth { get; set; } = START_HEALTH;

        public Data.GameLocation Location { get; set; } = Data.GameLocation.Værtshuset;

        public List<string> Log { get; set; } = new List<string>();

        public void UpdateLog(string entry)
        {
            Log.Add($"{DateTime.UtcNow.ToLongTimeString()}: {entry}");
        }
    }
}