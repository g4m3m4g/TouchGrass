# โฟลเดอร์: backend/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# คัดลอกไฟล์ .csproj จากโครงสร้างใหม่ภายในโฟลเดอร์ backend
COPY src/Core/BucketListApp.Domain/*.csproj ./src/Core/BucketListApp.Domain/
COPY src/Core/BucketListApp.Application/*.csproj ./src/Core/BucketListApp.Application/
COPY src/Infrastructure/BucketListApp.Infrastructure/*.csproj ./src/Infrastructure/BucketListApp.Infrastructure/
COPY src/Presentation/BucketListApp.WebApi/*.csproj ./src/Presentation/BucketListApp.WebApi/
RUN dotnet restore ./src/Presentation/BucketListApp.WebApi/BucketListApp.WebApi.csproj

# คัดลอกโค้ดทั้งหมด แล้วสั่ง Publish
COPY . ./
RUN dotnet publish ./src/Presentation/BucketListApp.WebApi/BucketListApp.WebApi.csproj -c Release -o out

# รัน Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "BucketListApp.WebApi.dll"]