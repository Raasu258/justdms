// See https://aka.ms/new-console-template for more information

using JustDMS.BlobStore;

internal class Main
{

    
    public static void main(string[] args)
    {
        string workingDir = Directory.GetCurrentDirectory();
        
        var DataStore = new BlobStore(workingDir);
        
    }
}