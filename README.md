# TouchGrass

A Bucket List WebApp Built with .NET in Clean Architecture

TECH STACK

- frontend : i will decide later hahahaha lol ~
- backend : .NET
- db : Oracle DB


## Project Structure
```text
BucketListApp/
├── BucketListApp.sln                       # Solution File สำหรับมัดรวมทุกโปรเจกต์
├── src/                                    # โฟลเดอร์เก็บ Source Code หลัก
│   ├── Core/
│   │   ├── BucketListApp.Domain/           # 1. Domain Layer (Class Library)
│   │   │   ├── Entities/                   # คลาสตัวแทนตาราง เช่น User.cs, BucketItem.cs
│   │   │   ├── Enums/                      # Enum ต่างๆ เช่น PriorityLevel.cs
│   │   │   └── Common/                     # Base Entity สำหรับทำ Audit Logs
│   │   │
│   │   └── BucketListApp.Application/      # 2. Application Layer (Class Library)
│   │       ├── Interfaces/                 # Interface สำหรับ Repositories / Services
│   │       ├── Services/                   # Business Logic Implementations
│   │       ├── DTOs/                       # Data Transfer Objects (Request/Response)
│   │       └── Exceptions/                 # Custom Exceptions สำหรับระบบ
│   │
│   ├── Infrastructure/
│   │   └── BucketListApp.Infrastructure/   # 3. Infrastructure Layer (Class Library)
│   │       ├── Data/                       # Oracle DbContext และ Migrations
│   │       ├── Repositories/               # โค้ดที่ติดต่อกับ Oracle DB จริงๆ
│   │       └── Security/                   # โค้ดจัดการ JWT, Password Hashing
│   │
│   └── Presentation/
│       └── BucketListApp.WebApi/           # 4. Presentation Layer (ASP.NET Web API)
│           ├── Controllers/                # API Endpoints (Auth, BucketListController)
│           ├── Middlewares/                # Custom Middleware เช่น ExceptionHandler
│           ├── Extensions/                 # Dependency Injection Setup Extensions
│           ├── appsettings.json            # Configuration สำหรับ Connection String/JWT
│           └── Program.cs                  # จุดเริ่มต้นของแอปพลิเคชัน (App Bootstrapper)
└── tests/                                  # โฟลเดอร์สำหรับเขียน Unit/Integration Tests (Phase 7)
```