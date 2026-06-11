using BucketListApp.Application.DTOs;
using BucketListApp.Application.Interfaces;
using BucketListApp.Domain.Entities;
using BucketListApp.Infrastructure.Data; // ในความเป็นจริงควรใช้ Repository แต่เพื่อลดความซับซ้อนในขั้นตอนนี้เราจะเรียกผ่าน DbContext ก่อน
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BucketListApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // 1. ตรวจสอบว่าอีเมลซ้ำหรือไม่
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            return false;

        // 2. แฮชรหัสผ่านด้วย BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. สร้าง User Entity (ขอบเขต ID จะถูกสร้างโดย Oracle SYS_GUID เองอัตโนมัติ)
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsActive = 1
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        // 1. ค้นหาผู้ใช้จากอีเมล
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || user.IsActive == 0) return null;

        // 2. ตรวจสอบรหัสผ่านว่าตรงกับในฐานข้อมูลไหม
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // 3. ถ้าถูกต้อง ให้สร้าง JWT Token ส่งกลับไป
        var token = GenerateJwtToken(user);
        return new AuthResponse(token, user.Email);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecret = _configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var key = Encoding.ASCII.GetBytes(jwtSecret);
        
        // แปลง byte[] UUID จาก Oracle ให้เป็นรูปแบบ String เพื่อเก็บลงใน Claim
        string userGuidString = new Guid(user.UserId).ToString();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, userGuidString),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(60), // Token มีอายุ 60 นาที
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}