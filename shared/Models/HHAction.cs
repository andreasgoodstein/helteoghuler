namespace HelteOgHulerShared.Models;

public abstract class HHAction
{
    public ActionName Name { get; set; }
    public string Outcome { get; set; }
    public ActionTarget Target { get; set; }
}
