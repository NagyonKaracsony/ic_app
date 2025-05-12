using Assets.Globals;
using Newtonsoft.Json.Linq;
public class SettingsHandler
{
    public static Settings settings = new();
    public static void SyncSettings()
    {
        AudioManager.SetMusicVolume(settings.MusicVolume);
        AudioManager.SetEffectsVolume(settings.EffectsVolume);
    }
    public static void SaveSettings()
    {
        JObject settingsObject = JObject.FromObject(settings);
        System.IO.File.WriteAllText(GlobalUtility.GameFolder + "/Settings.json", settingsObject.ToString());
    }
    public static void LoadSettings()
    {
        JObject settingsObject = JObject.Parse(System.IO.File.ReadAllText(GlobalUtility.GameFolder + "/Settings.json"));
        settings = settingsObject.ToObject<Settings>();
    }
}
public class Settings
{
    public float MusicVolume;
    public float EffectsVolume;
}