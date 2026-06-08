using BucketListApp.Application.Interfaces;
using BucketListApp.Application.Services;
using BucketListApp.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // ยอมรับพอร์ตหน้าบ้าน Angular ของเรา
              .AllowAnyMethod()                    // ยอมรับทุก Method (GET, POST, PUT, DELETE, OPTIONS)
              .AllowAnyHeader()                    // ยอมรับทุก Headers (รวมถึง Authorization, Content-Type)
              .AllowCredentials();                 // อนุญาตให้แนบ Cookie/Credentials หากจำเป็น
    });
});

// Load secret configuration
builder.Configuration.AddJsonFile("appsettings.Secret.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register Infrastructure Services (DbContext, Repositories, etc.)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBucketListService, BucketListService>();

// คอนฟิกและลงทะเบียน JWT Authentication System
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "jwt_typeshi_gang_67";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,   
        ValidateAudience = false, 
        ClockSkew = TimeSpan.Zero 
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp"); //เปิดใช้งาน CORS Middleware
app.UseAuthentication(); // แกะ JWT Token ตรวจสอบว่าเป็นใคร
app.UseAuthorization();  // ตรวจสอบว่ามีสิทธิ์เข้าถึง Endpoint นั้นไหม
app.MapControllers();

var summaries = new[]
{
    "gamie", "kubppom"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
