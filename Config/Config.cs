namespace JustDMS.Config;



public class Config
{
    private string configPath {get; }
    private string workingDir { get; }

    private Config()
    {
        //string appDir =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appDir = Directory.GetCurrentDirectory();
        configPath = Path.Combine(appDir, "Config");
    }
    
    
}