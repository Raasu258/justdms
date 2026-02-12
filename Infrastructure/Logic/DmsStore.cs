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
    
    internal bool ImportFile(Stream content, string fileName, string folderID = "root", int ownerId = 0, int tenantId = 0)
    {
        try
        {
            var hash = _blobStore.Put(content);
            using SqliteConnection db = _database.OpenConnection();
            using var cmd = db.CreateCommand();

            cmd.CommandText = @"
                
                INSERT INTO documents (
                    document_id,
                    folder_id,
                    name,
                    owner_id,
                    filetype             
                ) VALUES (
                     $documentId,
                     $folderId,
                     $fileName,
                     $owner_id,
                     $tenant_id, 
                     $fileType,   
                )                                  
            ";
            
            cmd.Parameters.AddWithValue("$documentId", Guid.NewGuid().ToString("N"));
            cmd.Parameters.AddWithValue("$folderID", folderID);
            cmd.Parameters.AddWithValue("$fileName", fileName ?? content.Name);
            cmd.Parameters.AddWithValue("$ownerId", ownerId);
            cmd.Parameters.AddWithValue("$tenantId", tenantId);
            cmd.Parameters.AddWithValue("$fileType", "test");

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
    
    internal bool CreateFolder(string folderName, string parentFolderID = "root", int ownerId = 0, int tenantId = 0)
    {
        try
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
          $parentFolderID,
          $folderName,
          $ownerId,
          $tenantId
        );

        ";

            cmd.Parameters.AddWithValue("$folderId", Guid.NewGuid().ToString("N"));
            cmd.Parameters.AddWithValue("$parentFolderID", parentFolderID);
            cmd.Parameters.AddWithValue("$folderName", folderName);
            cmd.Parameters.AddWithValue("$ownerId", ownerId);
            cmd.Parameters.AddWithValue("$tenantId", tenantId);

            cmd.ExecuteNonQuery();
        }
        catch (Exception e) // will add more specific exceptions in the future
        {
            return false;
        }

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
    
    internal void ListDocuments()
    {
        using SqliteConnection db = _database.OpenConnection();
        using var cmd = db.CreateCommand();

        cmd.CommandText = @"";
    }
    
    internal void GetOpenPath()
    {
        
    }
    
    
    
    
    
}