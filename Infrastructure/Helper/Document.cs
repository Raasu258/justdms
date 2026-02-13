namespace JustDMS.Infrastructure.Helper;

public class Document
{
    private string DocumentId { get; set; }
    private string FolderId { get; set; }
    private string DocumentName { get; set; }
    private int OwnerId { get; set; }
    private int TenantId { get; set; }
    private string Filetype { get; set; }

    public Document(string documentId, string folderId, string documentName, int ownerId, int tenantId, string filetype)
    {
        this.DocumentId = documentId;
        this.FolderId = folderId;
        this.DocumentName = documentName;
        this.OwnerId = ownerId;
        this.TenantId = tenantId;
        this.Filetype = filetype;
    }
}