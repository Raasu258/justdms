// See https://aka.ms/new-console-template for more information

using JustDMS;
using JustDMS.BlobStore;
using JustDMS.Database;
internal class Program
{
    
    public static void Main(string[] args)
    {
        string workingDir = Directory.GetCurrentDirectory();
        string blobStoreDir = Path.Combine(workingDir, "blobstore");
        
        var DataStore = new BlobStore(blobStoreDir);
        var Database = new Database(workingDir);
        
        using var fs = File.OpenRead("C:\\Users\\oelar\\Pictures\\test.txt");
        string hash = DataStore.Put(fs);

        Console.WriteLine(hash);
        Console.WriteLine(DataStore.Exists(hash));

        using var readBack = DataStore.Get(hash);
        Console.WriteLine(readBack.Length); // sollte nicht crashen
        Console.ReadLine();
        
    }
}