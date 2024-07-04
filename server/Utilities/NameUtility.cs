namespace HelteOgHulerServer.Utilities;

public class NameUtility
{
    private Random random;
    private readonly string[] NameList = ["Zindariel", "Bolgur", "Skadink", "Frog", "Hella", "Nyx", "Troy", "Declan"];
    private readonly string[] TitleList = ["the Brave", "the Quick", "the Strong", "the Magnificent", "the Greedy", "the Loud", "the Rich"];

    public NameUtility()
    {
        random = new Random();
    }

    public string GenerateName()
    {
        return $"{NameList[random.Next(0, NameList.Length)]} {TitleList[random.Next(0, TitleList.Length)]}";
    }
}