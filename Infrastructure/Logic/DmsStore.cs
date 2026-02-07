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
        catch(Exception e)
        {
            
        }
        

        return false;
    }
    internal bool ExportFile(string fileName, string path)
    {
        
    }
    internal bool CreateFolder(string folderName)
    {
        
    }
    internal bool ExportFolder(string folderName, string path)
    {
        
    }
    internal bool DeleteFolder(string folderName)
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