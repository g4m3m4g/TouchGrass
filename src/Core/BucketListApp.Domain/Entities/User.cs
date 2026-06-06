namespace BucketListApp.Domain.Entities;

public class User
{
    // กำหนดให้เป็น byte[] เพื่อรองรับ RAW(16) UUID จาก Oracle
    public byte[] UserId { get; set; } = Array.Empty<byte>();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int IsActive { get; set; } = 1;

    // Navigation Properties
    public ICollection<BucketListItem> BucketListItems { get; set; } = new List<BucketListItem>();
}