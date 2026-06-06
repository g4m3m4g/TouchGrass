using Microsoft.EntityFrameworkCore;
using BucketListApp.Domain.Entities;

namespace BucketListApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BucketListItem> BucketListItems => Set<BucketListItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // คอนฟิกตาราง USERS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("USER_ID").HasColumnType("RAW(16)").HasDefaultValueSql("SYS_GUID()");
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH").HasMaxLength(500).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("SYSTIMESTAMP");
            entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").HasDefaultValue(1);
        });

        // คอนฟิกตาราง BUCKET_LIST_ITEMS
        modelBuilder.Entity<BucketListItem>(entity =>
        {
            entity.ToTable("BUCKET_LIST_ITEMS");
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID").UseIdentityColumn();
            entity.Property(e => e.UserId).HasColumnName("USER_ID").HasColumnType("RAW(16)");
            entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Title).HasColumnName("TITLE").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(1000);
            entity.Property(e => e.Priority).HasColumnName("PRIORITY").HasMaxLength(10).IsRequired();
            entity.Property(e => e.IsCompleted).HasColumnName("IS_COMPLETED").HasDefaultValue(0);
            entity.Property(e => e.CompletedAt).HasColumnName("COMPLETED_AT");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("SYSTIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasDefaultValue(0);

            // ARCHITECT TRICK: Global Query Filter สำหรับ Soft Delete
            // ทุกครั้งที่เขียน Linq ดึงข้อมูลจากตารางนี้ มันจะเติม "WHERE IS_DELETED = 0" ให้เราเองโดยอัตโนมัติ
            entity.HasQueryFilter(e => e.IsDeleted == 0);

            // ตั้งค่าความสัมพันธ์ (Relationships)
            entity.HasOne(d => d.User)
                  .WithMany(p => p.BucketListItems)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}