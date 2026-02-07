using System.Reflection.Metadata;
using JustDMS.Infrastructure.Logic;

namespace JustDMS.Bootstrap;

using Config;
using Database;
using BlobStore;

internal class Bootstrap
{
    internal AppConfig AppConfig { get; set; }
    private Database Database { get; set; }
    private BlobStore BlobStore { get; set; }
    internal DmsStore DmsStore { get; set; }
    
    internal Bootstrap()
    {
        AppConfig = new AppConfig();
        Database = new Database(AppConfig.WorkingDir);
        BlobStore = new BlobStore(AppConfig.WorkingDir);
        DmsStore = new DmsStore(Database, BlobStore);
    }

    internal void Initialize()
    {
        try
        {
            Database.InitializeDb();
            BlobStore.InitializeBlobStore();
            DmsStore.Initialize();
        }
        catch (Exception e)
        {   // TEMPORARY FOR TESTING!!! will add logger
            Console.WriteLine("ERROR: " + e.ToString());
        }
    }
    
}