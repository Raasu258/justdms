using JustDMS.Infrastructure.Helper;
using Microsoft.Data.Sqlite;

namespace JustDMS.Infrastructure.Logic;

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
          foldername,
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
        
        cmd.ExecuteNonQuery();
    }
    
    internal bool ImportFile(FileStream content, string fileName = "", string folderId = "root", int ownerId = 0, int tenantId = 0)
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
                    docname,
                    owner_id,
                    tenant_id,
                    filetype             
                ) VALUES (
                     $documentId,
                     $folderId,
                     $fileName,
                     $owner_id,
                     $tenant_id, 
                     $fileType   
                );         

                INSERT INTO versions (
                    version_id,
                    document_id,
                    blob_hash,
                    created_at_utc,
                    base_version_id
                ) VALUES (
                    $versionId,
                    $documentId,
                    $blobHash,
                    $createdAt,
                    0
                );
            ";
            
            
            var documentId = Guid.NewGuid().ToString("N");
            var versionId = Guid.NewGuid().ToString("N");
            var createdAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            
            cmd.Parameters.AddWithValue("$documentId", documentId);
            cmd.Parameters.AddWithValue("$folderID", folderId);
            cmd.Parameters.AddWithValue("$fileName", fileName == "" ? Path.GetFileName(content.Name) : fileName);
            cmd.Parameters.AddWithValue("$ownerId", ownerId);
            cmd.Parameters.AddWithValue("$tenantId", tenantId);
            cmd.Parameters.AddWithValue("$fileType", "test");
            
            cmd.Parameters.AddWithValue("$versionId", versionId);
            cmd.Parameters.AddWithValue("$blobHash", hash);
            cmd.Parameters.AddWithValue("$createdAt", createdAt);
            
            cmd.ExecuteNonQuery();

        }
        catch (ArgumentNullException)   // idea for future return of an array with error to make a good error message in GUI
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
        
        return true;
    }
    internal bool ExportFile(string fileName, string path)
    {
        
        
        return false;
    }
    
    internal bool CreateFolder(string folderName, string parentFolderId = "root", int ownerId = 0, int tenantId = 0)
    {
        try
        {
            using SqliteConnection db = _database.OpenConnection();
            using var cmd = db.CreateCommand();

            cmd.CommandText = @"
        INSERT INTO folders (
          folder_id,
          parent_folder_id,
          foldername,
          owner_id,
          tenant_id
        )
        VALUES (
          $folderId, 
          $parentFolderID,
          $folderName,
          $ownerId,
          $tenantId
        );

        ";

            cmd.Parameters.AddWithValue("$folderId", Guid.NewGuid().ToString("N"));
            cmd.Parameters.AddWithValue("$parentFolderID", parentFolderId);
            cmd.Parameters.AddWithValue("$folderName", folderName);
            cmd.Parameters.AddWithValue("$ownerId", ownerId);
            cmd.Parameters.AddWithValue("$tenantId", tenantId);

            cmd.ExecuteNonQuery();
        }
        catch (Exception e) // will add more specific exceptions in the future
        {
            return false;
        }

        return true;
    }
    
    internal bool ExportFolder(string folderName, string path)
    {
        return false;
    }
    
    internal bool DeleteFolder(string folderName)
    {
        return false;
    }
    
    internal bool DeleteDocument(string fileName)
    {
        return false;
    }
    
    internal void ListDocuments()
    {
        using SqliteConnection db = _database.OpenConnection();
        using var cmd = db.CreateCommand();
        
        cmd.CommandText = @"

        
        
        ";
        
        var values = new List<Document>();
        using (var reader = cmd.ExecuteReader()){
            while (reader.Read())
            {
                values.Add(new Document(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5)));
            }
        
        
        }
    }

    internal void ListFolders()
    {
        using SqliteConnection db = _database.OpenConnection();
        using var cmd = db.CreateCommand();
        
        cmd.CommandText = @"
            
        
        
        ";

        var values = new List<Folder>();
        
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                values.Add(new Folder(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4)));
            }
        }
    }
    
    internal void GetOpenPath()
    {
        
    }
    
    
    
    
    
}