namespace BucketListApp.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public byte[]? UserId { get; set; } = Array.Empty<byte>();
    public string Name { get; set; } = string.Empty;

    public int IsDeleted { get; set; } = 0;

    // Navigation Properties
    public User? User { get; set; }
    public ICollection<BucketListItem> BucketListItems { get; set; } = new List<BucketListItem>();
}
