using System.Text;

namespace JustDMS.BlobStore;

using System.IO;
using System.Security.Cryptography;

internal class BlobStore
{
    private readonly string _rootDir;

    public BlobStore(string rootDirectory)
    {
        _rootDir = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        
    }

    internal void InitializeBlobStore()
    {
        Directory.CreateDirectory(_rootDir);
    }
    
    
    internal string Put(Stream content)
    {
        ArgumentNullException.ThrowIfNull(content);;
        if(!content.CanRead) throw new ArgumentException("Content must be readable", nameof(content));
        
        string tmpPath = Path.Combine(_rootDir, ".tmp-" + Guid.NewGuid().ToString("N") + ".bin");

        using var sha = SHA256.Create();
        
        using (var tmp = new FileStream(tmpPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            byte[] buffer = new byte[64 * 1024]; // 64KB
            int read;

            while ((read = content.Read(buffer, 0, buffer.Length)) > 0)
            {
                sha.TransformBlock(buffer, 0, read, null, 0);
                
                tmp.Write(buffer, 0, read);
            }
            
            sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        }
        
        string hash = Convert.ToHexString(sha.Hash!).ToLowerInvariant();
        
        string finalPath = GetBlobPath(hash);
        Directory.CreateDirectory(Path.GetDirectoryName(finalPath)!);
        
        if (File.Exists(finalPath))
        {
            File.Delete(tmpPath);
            return hash;
        }

        try
        {
            File.Move(tmpPath, finalPath);
        }
        catch (IOException)
        {
            File.Delete(tmpPath);
        }
        return hash;
        
    }

    internal Stream Get(string hash)
    {
        string path = GetBlobPath(hash);

        if (!File.Exists(path))
            throw new FileNotFoundException("Blob not found.", path);

        return File.OpenRead(path);
    }

    internal bool Exists(string hash)
    {
        string path = GetBlobPath(hash);
        return File.Exists(path);
    }
    
    private string GetBlobPath(string hash)
    {
        ValidateHash(hash);

        string p1 = hash.Substring(0, 2);
        string p2 = hash.Substring(2, 2);

        return Path.Combine(_rootDir, p1, p2, hash + ".bin");
    }

    private static void ValidateHash(string hash)
    {
        ArgumentNullException.ThrowIfNull(hash);
        if (hash.Length != 64) throw new ArgumentException("SHA-256 hex must be 64 chars.", nameof(hash));
        
        foreach (var c in hash)
        {
            var isHex =
                (c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');

            if (!isHex) throw new ArgumentException("Invalid hex chars in hash.", nameof(hash));
        }
        
    }
    
}
