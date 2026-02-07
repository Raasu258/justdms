namespace JustDMS.Config;

internal class AppConfig
{
    internal string ConfigPath { get; }
    internal string WorkingDir { get; }

    internal AppConfig()
    {
        //string appDir =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        WorkingDir = Directory.GetCurrentDirectory();
        ConfigPath = Path.Combine(WorkingDir, "Config");
    }
    
    
}