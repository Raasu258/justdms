namespace JustDMS.Infrastructure.Helper;

public class Folder
{
    private string FolderId { get; set; }
    private string ParentId { get; set; }
    private string Foldername { get; set; }
    private int OwnerId { get; set; }
    private int TenantId { get; set; }

    public Folder(string folderId, string parentId, string folderName, int ownerId, int tenantId)
    {
        this.FolderId = folderId;
        this.ParentId = parentId;
        this.Foldername = folderName;
        this.OwnerId = ownerId;
        this.TenantId = tenantId;
    } 
    
}