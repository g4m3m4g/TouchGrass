using BucketListApp.Application.DTOs;

namespace BucketListApp.Application.Interfaces;

public interface IBucketListService
{
    Task<BucketItemResponse> CreateItemAsync(byte[] userId, CreateBucketItemRequest request);
    Task<IEnumerable<BucketItemResponse>> GetUserItemsAsync(byte[] userId);
    Task<BucketItemResponse?> GetItemByIdAsync(int itemId, byte[] userId);
    Task<bool> UpdateItemAsync(int itemId, byte[] userId, UpdateBucketItemRequest request);
    Task<bool> DeleteItemAsync(int itemId, byte[] userId);
    Task<BucketItemResponse?> PickRandomItemAsync(byte[] userId); // ฟีเจอร์สุ่ม
}