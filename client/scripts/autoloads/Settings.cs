using Godot;

namespace HelteOgHulerClient;

public class Settings : Node
{
    const string fileName = "user://settings.ini";

    private readonly ConfigFile settings;

    public string LoginName
    {
        get
        {
            return (string)settings.GetValue("", "loginName", "");
        }
        set
        {
            settings.SetValue("", "loginName", value);
            Save();
        }
    }

    public bool ConfirmationEnabled
    {
        get
        {
            return (bool)settings.GetValue("", "confirmationEnabled", true);
        }
        set
        {
            settings.SetValue("", "confirmationEnabled", value);
            Save();
        }
    }

    public Settings()
    {
        settings = new ConfigFile();
        settings.Load(fileName);
    }

    private void Save()
    {
        settings.Save(fileName);
    }
}
