using System.Runtime.InteropServices.ComTypes;

namespace JustDMS.Database;

using Microsoft.Data.Sqlite;

internal sealed class Database
{
    private readonly string _dbPath;
    private readonly string _connectionString;
    
    internal Database(string repoRootPath)
    {
        if (string.IsNullOrWhiteSpace(repoRootPath))
            throw new ArgumentException("Repo root path is required.", nameof(repoRootPath));
        
        _dbPath = Path.Combine(repoRootPath, "Database", "justdms_db.db");
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = _dbPath,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();
        
    }

    internal void InitializeDb()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_dbPath)!);
        InitializeSchema();
    }

    internal SqliteConnection OpenConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                          PRAGMA foreign_keys = ON;
                          PRAGMA journal_mode = WAL;
                          PRAGMA busy_timeout = 5000;
                          """;
        cmd.ExecuteNonQuery();
        return conn;
    }

    private void InitializeSchema()
    {
        using var connection = this.OpenConnection();
        using var tx = connection.BeginTransaction();
        using var command = connection.CreateCommand();
        command.Transaction = tx;
        
        // to enforce foreign keys
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        command.CommandText = @"

            CREATE TABLE IF NOT EXISTS tenants(
                tenant_id TEXT PRIMARY KEY,
                name TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS users(
                uid TEXT PRIMARY KEY,
                tenant_id TEXT NOT NULL,
                FOREIGN KEY(tenant_id) REFERENCES tenants(tenant_id)
             );

            CREATE TABLE IF NOT EXISTS folders (
              folder_id TEXT PRIMARY KEY,
              parent_folder_id TEXT NULL,
              name TEXT NOT NULL,
              owner_id INTEGER NOT NULL,
              tenant_id INTEGER NOT NULL,
              FOREIGN KEY(parent_folder_id) REFERENCES folders(folder_id),
              FOREIGN KEY(owner_id) REFERENCES users(uid),
              FOREIGN KEY (tenant_id) REFERENCES tenants(tenant_id)
            );

            CREATE TABLE IF NOT EXISTS documents (
              document_id TEXT PRIMARY KEY,
              folder_id TEXT NOT NULL,
              name TEXT NOT NULL,
              owner_id INTEGER NOT NULL,
              tenant_id INTEGER NOT NULL,
              filetype TEXT NULL,
              is_deleted INTEGER NOT NULL DEFAULT 0,
              FOREIGN KEY(folder_id) REFERENCES folders(folder_id),
              FOREIGN KEY(owner_id) REFERENCES users(uid),
              FOREIGN KEY (tenant_id) REFERENCES tenants(tenant_id)
            );

            CREATE TABLE IF NOT EXISTS versions (
              version_id TEXT PRIMARY KEY,
              document_id TEXT NOT NULL,
              blob_hash TEXT NOT NULL,
              created_at_utc TEXT NOT NULL,
              base_version_id TEXT NULL,
              FOREIGN KEY(document_id) REFERENCES documents(document_id)
            );

            CREATE TABLE IF NOT EXISTS metadata(
                
                metadata_id TEXT PRIMARY KEY,
                document_id TEXT NOT NULL,
                category TEXT,
                label TEXT,
                FOREIGN KEY(document_id) REFERENCES documents(document_id)
            );

            INSERT OR IGNORE INTO tenants(tenant_id, name) VALUES(0, 'default');
            INSERT OR IGNORE INTO users(uid, tenant_id) VALUES(0, 0);

            CREATE INDEX IF NOT EXISTS idx_documents_folder ON documents(folder_id);
            CREATE INDEX IF NOT EXISTS idx_versions_document ON versions(document_id);
            CREATE INDEX IF NOT EXISTS idx_folders_owner_parent ON folders(owner_id, parent_folder_id);
            CREATE INDEX IF NOT EXISTS idx_documents_owner_folder_active ON documents(owner_id, folder_id) WHERE is_deleted = 0;
            CREATE INDEX IF NOT EXISTS idx_versions_document_created ON versions(document_id, created_at_utc DESC);
            
            ";
            
            command.ExecuteNonQuery();
            
            tx.Commit();
    }
    
    public string DbPath => _dbPath;
    
}