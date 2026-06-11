using BucketListApp.Application.DTOs;
using BucketListApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BucketListApp.WebApi.Controllers;

[Authorize] //บังคับว่าทุก Endpoint ใน Controller นี้ต้องแนบ JWT Token ที่ถูกต้องมาด้วย
[ApiController]
[Route("api/[controller]")]
public class BucketListController : ControllerBase
{
    private readonly IBucketListService _bucketService;

    public BucketListController(IBucketListService bucketService)
    {
        _bucketService = bucketService;
    }

    // Helper Method ส่วนตัวสำหรับแกะ ID ของผู้ใช้ปัจจุบันจาก JWT Token ออกมาเป็น byte[] สำหรับ Oracle DB
    private byte[] GetCurrentUserId()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Array.Empty<byte>();
        return Guid.Parse(userIdStr).ToByteArray();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBucketItemRequest request)
    {
        var userId = GetCurrentUserId();
        var response = await _bucketService.CreateItemAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = response.ItemId }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var items = await _bucketService.GetUserItemsAsync(userId);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetCurrentUserId();
        var item = await _bucketService.GetItemByIdAsync(id, userId);
        if (item == null) return NotFound(new { message = "Item not found or access denied." });
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBucketItemRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _bucketService.UpdateItemAsync(id, userId, request);
        if (!result) return NotFound(new { message = "Item not found or cannot be updated." });
        return Ok(new { message = "Item updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _bucketService.DeleteItemAsync(id, userId);
        if (!result) return NotFound(new { message = "Item not found." });
        return Ok(new { message = "Item deleted successfully (Soft Delete)." });
    }

    [HttpGet("pick-random")]
    public async Task<IActionResult> PickRandom()
    {
        var userId = GetCurrentUserId();
        var item = await _bucketService.PickRandomItemAsync(userId);
        if (item == null) return BadRequest(new { message = "No uncompleted bucket list items found to pick from." });
        return Ok(item);
    }
}