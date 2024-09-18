#nullable enable

using HelteOgHulerShared.Models;

namespace HelteOgHulerShared.Interfaces;

public interface IEncounterActor
{
    public Guid Id { get; }
    public string Name { get; }
    public int HP { get; set; }
    // public void SetStatus();
    public void TakeAction(Encounter encounter, Random? random);
}
