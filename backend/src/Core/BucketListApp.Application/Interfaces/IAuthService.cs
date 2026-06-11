using BucketListApp.Application.DTOs;

namespace BucketListApp.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}