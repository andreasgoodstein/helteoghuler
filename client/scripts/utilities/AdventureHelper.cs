using HelteOgHulerShared.Models;
using System.Linq;

namespace HelteOgHulerClient.Utilities;

public static class AdventureHelper
{
    public static string GetAdventureText(Adventure adventure)
    {
        if (adventure == null)
        {
            return "No previous adventure recorded.";
        }

        string result = "";

        result += $"{GetPartyNames(adventure.Party)} went on an adventure.\n";
        result += $"They {adventure.Status}! And returned with {adventure.Gold} gold.\n\n";

        foreach (var encounter in adventure.EncounterList)
        {
            result += $"The Party encountered {encounter.Monster.Name}\n\n";

            foreach (var action in encounter.ActionLog)
            {
                result += $"{action}\n";
            }
        }

        return result;
    }

    private static string GetPartyNames(Hero[] party)
    {
        if (party.Length < 2)
        {
            return party.FirstOrDefault()?.Name ?? "";
        }

        string result = "";

        for (int n = 0; n < party.Length; n += 1)
        {
            var hero = party[n];

            if (n == 0)
            {
                result += $"{hero.Name}";
                continue;
            }

            if (n == party.Length - 1)
            {
                result += $", and {hero.Name}";
                break;
            }

            result += $", {hero.Name}";
        }

        return result;
    }
}
