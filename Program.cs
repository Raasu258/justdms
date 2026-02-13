// See https://aka.ms/new-console-template for more information

using JustDMS;
using JustDMS.BlobStore;
using JustDMS.Bootstrap;
using JustDMS.Config;
using JustDMS.Database;
internal class Program
{
    
    public static void Main(string[] args)
    {
        string workingDir = Directory.GetCurrentDirectory();
        string blobStoreDir = Path.Combine(workingDir, "blobstore");

        var bootstrap = new Bootstrap();
        bootstrap.Initialize();
        
        var config = new AppConfig();
        var dataStore = new BlobStore(blobStoreDir);
        var database = new Database(workingDir);

        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string filePath = "";
        
        if(OperatingSystem.IsWindows()){
            
            filePath = Path.Combine(baseDir, "Pictures", "test.txt");
            
        }else if(OperatingSystem.IsLinux()){
            
            filePath = Path.Combine(baseDir, "Pictures", "test.txt");
            
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
        
        using var fs = File.OpenRead(filePath);
        string hash = dataStore.Put(fs);
        Console.WriteLine(hash);
        Console.WriteLine(dataStore.Exists(hash));

        using var readBack = dataStore.Get(hash);
        Console.WriteLine(readBack.Length); // sollte nicht crashen
        
        using var reader = new StreamReader(readBack);
        Console.WriteLine(reader.ReadToEnd());
        Console.ReadLine();
        
    }
}