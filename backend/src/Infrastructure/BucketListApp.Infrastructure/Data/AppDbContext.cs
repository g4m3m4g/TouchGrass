using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BucketListApp.Domain.Entities;
using BucketListApp.Domain.Enums;

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

        // Value converter for Oracle (int boolean: 0 = false, 1 = true)
        var intToBoolConverter = new ValueConverter<int, int>(
            v => v == 0 ? 0 : 1,
            v => v
        );

        // คอนฟิกตาราง USERS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId)
                .HasColumnName("USER_ID")
                .HasColumnType("RAW(16)")
                .HasDefaultValueSql("SYS_GUID()")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH").HasMaxLength(500).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("SYSTIMESTAMP");
            entity.Property(e => e.IsActive)
                .HasColumnName("IS_ACTIVE")
                .HasDefaultValue(1)
                .HasConversion(intToBoolConverter);
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
            entity.Property(e => e.Priority)
                .HasColumnName("PRIORITY")
                .HasMaxLength(10)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(), // ตอนเซฟลง DB ให้แปลง Enum เป็น String
                    v => Enum.Parse<PriorityLevel>(v) // ตอนอ่านจาก DB ให้แปลง String เป็น Enum
                );
            entity.Property(e => e.IsCompleted)
                .HasColumnName("IS_COMPLETED")
                .HasDefaultValue(0)
                .HasConversion(intToBoolConverter);
            entity.Property(e => e.CompletedAt).HasColumnName("COMPLETED_AT");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("SYSTIMESTAMP");
            entity.Property(e => e.IsDeleted)
                .HasColumnName("IS_DELETED")
                .HasDefaultValue(0)
                .HasConversion(intToBoolConverter);

            // ARCHITECT TRICK: Global Query Filter สำหรับ Soft Delete
            // ทุกครั้งที่เขียน Linq ดึงข้อมูลจากตารางนี้ มันจะเติม "WHERE IS_DELETED = 0" ให้เราเองโดยอัตโนมัติ
            entity.HasQueryFilter(e => e.IsDeleted == 0);

            // ตั้งค่าความสัมพันธ์ (Relationships)
            entity.HasOne(d => d.User)
                  .WithMany(p => p.BucketListItems)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Category)
              .WithMany(p => p.BucketListItems)
              .HasForeignKey(d => d.CategoryId)
              .OnDelete(DeleteBehavior.SetNull); // คล้องกับ ON DELETE SET NULL ในสคริปต์ SQL
        });
    
    // คอนฟิกตาราง CATEGORIES
    modelBuilder.Entity<Category>(entity =>
    {
        entity.ToTable("CATEGORIES");
        entity.HasKey(e => e.CategoryId);
        entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID").UseIdentityColumn();
        entity.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(100).IsRequired();
        
        // รองรับการเก็บ UUID ของเจ้าของหมวดหมู่ (สำหรับแนวทาง Hybrid)
        entity.Property(e => e.UserId).HasColumnName("USER_ID").HasColumnType("RAW(16)");
        
        entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasDefaultValue(0);

        // Global Query Filter สำหรับซ่อนหมวดหมู่ที่โดน Soft Delete
        entity.HasQueryFilter(e => e.IsDeleted == 0);

        // ใส่เผื่อไว้เพื่อผูกมัดความสัมพันธ์ฝั่ง User ไม่ให้แอบเจน USER_ID1 ขึ้นมา
        entity.HasOne(d => d.User)
          .WithMany()
          .HasForeignKey(d => d.UserId)
          .OnDelete(DeleteBehavior.Cascade);
    });
    }
}