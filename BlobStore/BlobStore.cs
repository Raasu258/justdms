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
        Directory.CreateDirectory(_rootDir);
    }
    

    internal string Put(Stream content)
    {
        
    }

    internal Stream Get(string hash)
    {
        
    }

    internal bool Exists(string hash)
    {
        
    }
    
}
