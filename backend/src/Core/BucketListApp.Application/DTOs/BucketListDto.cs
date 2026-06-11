namespace BucketListApp.Application.DTOs;

// ใช้รับข้อมูลตอนสร้างไอเทมใหม่
public record CreateBucketItemRequest(
    string Title, 
    string? Description, 
    string Priority, // LOW, MEDIUM, HIGH
    int? CategoryId
);

// ใช้รับข้อมูลตอนสั่งแก้ไขไอเทม
public record UpdateBucketItemRequest(
    string Title, 
    string? Description, 
    string Priority, 
    int? CategoryId,
    int IsCompleted
);

// ใช้ส่งข้อมูลกลับไปแสดงผลที่หน้าบ้าน (แปลง RAW(16) ออกเป็นอักษร String เพื่อให้หน้าบ้านใช้ง่าย)
public record BucketItemResponse(
    int ItemId,
    string Title,
    string? Description,
    string Priority,
    int IsCompleted,
    DateTime? CompletedAt,
    DateTime CreatedAt,
    int? CategoryId,
    string? CategoryName
);