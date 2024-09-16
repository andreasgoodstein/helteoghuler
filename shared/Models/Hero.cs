using HelteOgHulerShared.Interfaces;
using System.Runtime.Serialization;

#if (NET6_0_OR_GREATER)
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
#endif

namespace HelteOgHulerShared.Models;

public class Hero : IEncounterActor
{
#if (NET6_0_OR_GREATER)
    [BsonIgnore]
    [JsonIgnore]
#endif
    [IgnoreDataMember]
    public ulong HP { get; set; }
    public ulong MaxHP { get; set; } = 2;
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ulong Price { get; set; }
    public List<ActionName> ActionList { get; set; } = [.. Actions.DefaultActions.Keys];

#if (NET6_0_OR_GREATER)
    [BsonIgnore]
    [JsonIgnore]
#endif
    [IgnoreDataMember]
    private readonly Dictionary<AttackModifier, int> AttackModifiers = new() {
            { AttackModifier.ToCrit, -5 },
            { AttackModifier.ToHit, -5 }
        };

    public void TakeAction(Encounter encounter, Random random)
    {
        Actions.Attack.TakeAction(encounter, AttackModifiers, random);

        // TODO: Implement ability selection
        // Random dice = new Random();

        // var roll = dice.Next(1, 100);

        // if (roll <= 75)
        // {
        //     ActionList[ActionName.Attack].TakeAction(encounter);
        // }
        // else
        // {
        //     ActionList[ActionName.Dodge].TakeAction(encounter);
        // }
    }
}
