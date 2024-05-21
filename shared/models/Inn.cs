using System.Collections.Generic;

namespace HelteOgHulerShared.Models
{
    public class Inn
    {
        public Chest Chest { get; set; }

        public IEnumerable<Hero> HeroRoster { get; set; }
    }
}