namespace HelteOgHulerShared.Models;

public class Inn
{
    public List<InnUpgradeName> AvailableUpgrades { get; set; } = [];
    public List<InnUpgradeName> BuiltUpgrades { get; set; } = [];
    public Chest Chest { get; set; }
    public Dictionary<string, Hero> HeroRecruits { get; set; }
    public Dictionary<string, Hero> HeroRoster { get; set; }
    public string Name { get; set; }
    public InnUpgradeName? PendingUpgrade;
}

public enum InnUpgradeName
{
    DiscoverWorkshop = 0,
    RenovateWorkshop = 1,
    BuildShopLevel1 = 2,
}

public static class InnUpgrades
{
    public static readonly Dictionary<InnUpgradeName, ulong> Cost = new()
    {
        { InnUpgradeName.RenovateWorkshop, 25 },
        { InnUpgradeName.BuildShopLevel1, 50 },
    };

    public static readonly Dictionary<InnUpgradeName, InnUpgradeName[]> TechTree = new()
    {
        { InnUpgradeName.DiscoverWorkshop, [InnUpgradeName.RenovateWorkshop] },
        { InnUpgradeName.RenovateWorkshop, [InnUpgradeName.BuildShopLevel1] },
    };
}
