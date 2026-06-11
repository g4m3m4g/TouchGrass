using BucketListApp.Domain.Enums;

namespace BucketListApp.Domain.Entities;

public class BucketListItem
{
    public int ItemId { get; set; }
    public byte[] UserId { get; set; } = Array.Empty<byte>();
    public int? CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.MEDIUM;
    public int IsCompleted { get; set; } = 0;
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int IsDeleted { get; set; } = 0; // รองรับ Soft Delete

    // Navigation Properties
    public User? User { get; set; }
    public Category? Category { get; set; }
}