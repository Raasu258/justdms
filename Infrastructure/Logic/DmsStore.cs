using Microsoft.Data.Sqlite;

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
        using SqliteConnection db = _database.OpenConnection();
        using var cmd = db.CreateCommand();
        cmd.CommandText = @"

        INSERT OR IGNORE INTO folders (
          folder_id,
          parent_folder_id,
          name,
          owner_id,
          tenant_id
        )
        VALUES (
          'root',
          NULL,
          'Root',
          0,
          0
        )";
    }
    
    internal bool ImportFile(Stream content, int folderId = -1)
    {
        try
        {
            var hash = _blobStore.Put(content);
            using SqliteConnection db = _database.OpenConnection();
            using var cmd = db.CreateCommand();
            

            

        }
        catch (ArgumentNullException)   // idea for future return of array with error to make a good error message in GUI
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch(Exception e)
        {
            return false;
        }
        

        return false;
    }
    internal bool ExportFile(string fileName, string path)
    {
        
    }
    internal bool CreateFolder(string folderName)
    {
        using SqliteConnection db = _database.OpenConnection();
        using var cmd = db.CreateCommand();

        cmd.CommandText = @"
        INSERT INTO folders (
          folder_id,
          parent_folder_id,
          name,
          owner_id,
          tenant_id
        )
        VALUES (
          $folderId, // offen
          'root',
          $fileName,
          0,
          0
        );

        ";

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