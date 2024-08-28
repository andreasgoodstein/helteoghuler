using HelteOgHulerShared.Models;

namespace HelteOgHulerShared.Interfaces;

public interface IEncounterActor
{
    public string Name { get; }
    public ulong HP { get; set; }
    // public void SetStatus();
    public void TakeAction(Encounter encounter);
}
