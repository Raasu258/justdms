namespace JustDMS.Infrastructure.Logic;

using System.Collections.Generic;
using Bootstrap;
using BlobStore;
using Database;

internal class DmsStore
{
    private readonly Database _database;
    private readonly BlobStore _blobStore;
    
    internal DmsStore(Database database, BlobStore blobStore)
    {
        _database = database;
        _blobStore = blobStore;
    }
    
    internal void Initialize()
    {
        
    }

    internal bool ImportFile(string fileName)
    {
        try
        {
            
        }
        

        return false;
    }
    
    internal bool ExportFile(string fileName)
    {
        
    }

    internal bool DeleteDocument(string fileName)
    {
        
    }
    
    internal List<T> ListDocuments()
    {
    }

    internal void GetOpenPath()
    {
        
    }
    
    
    
}