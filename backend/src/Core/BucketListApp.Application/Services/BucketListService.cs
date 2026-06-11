using BucketListApp.Application.DTOs;
using BucketListApp.Application.Interfaces;
using BucketListApp.Domain.Entities;
using BucketListApp.Domain.Enums;
using BucketListApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BucketListApp.Application.Services;

public class BucketListService : IBucketListService
{
    private readonly AppDbContext _context;

    public BucketListService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BucketItemResponse> CreateItemAsync(byte[] userId, CreateBucketItemRequest request)
    {
        var item = new BucketListItem
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Priority = Enum.Parse<PriorityLevel>(request.Priority, true),
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.BucketListItems.Add(item);
        await _context.SaveChangesAsync();

        return new BucketItemResponse(
            item.ItemId, 
            item.Title, 
            item.Description, 
            item.Priority.ToString(), 
            item.IsCompleted, 
            item.CompletedAt, 
            item.CreatedAt, 
            item.CategoryId, null);
    }

    public async Task<IEnumerable<BucketItemResponse>> GetUserItemsAsync(byte[] userId)
    {
        return await _context.BucketListItems
            .Include(i => i.Category) // ทำ Eager Loading เพื่อเอาชื่อหมวดหมู่มาด้วย
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new BucketItemResponse(
                i.ItemId, 
                i.Title, 
                i.Description, 
                i.Priority.ToString(), 
                i.IsCompleted, 
                i.CompletedAt, 
                i.CreatedAt, 
                i.CategoryId, 
                i.Category != null ? i.Category.Name : null
            ))
            .ToListAsync();
    }

    public async Task<BucketItemResponse?> GetItemByIdAsync(int itemId, byte[] userId)
    {
        var i = await _context.BucketListItems
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.ItemId == itemId && i.UserId == userId);

        if (i == null) return null;

        return new BucketItemResponse(
            i.ItemId, 
            i.Title, 
            i.Description, 
            i.Priority.ToString(), 
            i.IsCompleted, 
            i.CompletedAt, 
            i.CreatedAt, 
            i.CategoryId, 
            i.Category?.Name);
    }

    public async Task<bool> UpdateItemAsync(int itemId, byte[] userId, UpdateBucketItemRequest request)
    {
        var item = await _context.BucketListItems.FirstOrDefaultAsync(i => i.ItemId == itemId && i.UserId == userId);
        if (item == null) return false;

        item.Title = request.Title;
        item.Description = request.Description;
        item.Priority = Enum.Parse<PriorityLevel>(request.Priority, true);
        item.CategoryId = request.CategoryId;

        // จัดการสถานะการทำสำเร็จ (ถ้าพึ่งทำเสร็จ ให้บันทึกเวลาปัจจุบัน)
        if (request.IsCompleted == 1 && item.IsCompleted == 0)
        {
            item.IsCompleted = 1;
            item.CompletedAt = DateTime.UtcNow;
        }
        else if (request.IsCompleted == 0)
        {
            item.IsCompleted = 0;
            item.CompletedAt = null;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItemAsync(int itemId, byte[] userId)
    {
        var item = await _context.BucketListItems.FirstOrDefaultAsync(i => i.ItemId == itemId && i.UserId == userId);
        if (item == null) return false;

        // ใช้ Soft Delete 
        item.IsDeleted = 1; 
        await _context.SaveChangesAsync();
        return true;
    }

    // Pick Random Feature
    public async Task<BucketItemResponse?> PickRandomItemAsync(byte[] userId)
    {
        // ดึงเฉพาะรายการที่ "ยังทำไม่สำเร็จ" ของผู้ใช้คนนี้
        var uncompletedItems = await _context.BucketListItems
            .Include(i => i.Category)
            .Where(i => i.UserId == userId && i.IsCompleted == 0)
            .ToListAsync();

        if (!uncompletedItems.Any()) return null;

        // ใช้คลาส Random ในการสุ่ม Index จากลิสต์ข้อมูลบน Memory บน Server
        var random = new Random();
        int randomIndex = random.Next(uncompletedItems.Count);
        var i = uncompletedItems[randomIndex];

        return new BucketItemResponse(
            i.ItemId, 
            i.Title, 
            i.Description, 
            i.Priority.ToString(), 
            i.IsCompleted, 
            i.CompletedAt, 
            i.CreatedAt, 
            i.CategoryId, 
            i.Category?.Name);
    }
}