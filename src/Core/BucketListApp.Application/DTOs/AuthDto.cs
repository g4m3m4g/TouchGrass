namespace BucketListApp.Application.DTOs;

// DTO สำหรับรับข้อมูลตอนสมัครสมาชิก
public record RegisterRequest(string Email, string Password);

// DTO สำหรับรับข้อมูลตอนเข้าสู่ระบบ
public record LoginRequest(string Email, string Password);

// DTO สำหรับส่ง Token กลับไปให้ Frontend
public record AuthResponse(string Token, string Email);