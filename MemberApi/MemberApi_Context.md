# 🚀 MemberApi Project Context (ASP.NET Core + Testing)

## 📌 Project Overview

This is a simple ASP.NET Core Web API project built to demonstrate:

* Clean architecture basics
* Repository pattern
* Service layer abstraction
* Dependency Injection
* Unit testing using xUnit and Moq

---

## 🧱 Project Structure

```
MemberSolution/
 ├── MemberApi/
 │    ├── Controllers/
 │    │     └── MembersController.cs
 │    ├── Models/
 │    │     └── Member.cs
 │    ├── Interfaces/
 │    │     └── IMembers.cs
 │    ├── Repositories/
 │    │     └── MembersRepository.cs
 │    ├── Services/
 │    │     ├── IMemberService.cs
 │    │     └── MemberService.cs
 │    ├── Program.cs
 │
 ├── MemberApi.Tests/
 │    ├── Services/
 │    │     └── MemberServiceTests.cs
```

---

## 🧩 Core Components

### 🔹 Model

```csharp
public class Member
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}
```

---

### 🔹 Repository Layer

#### Interface

```csharp
public interface IMembers
{
    List<Member> GetAllMembers();
    Member GetMember(int id);
}
```

#### Implementation

* Uses in-memory list
* Simulates database operations

---

### 🔹 Service Layer (Business Logic)

#### Interface

```csharp
public interface IMemberService
{
    Task<List<Member>> GetAllMembersAsync();
    Task<Member> GetMemberByIdAsync(int id);
}
```

#### Implementation

* Calls repository
* Handles business rules (e.g., throws exception if member not found)
* Uses async pattern (`Task.FromResult` for now)

---

### 🔹 Controller Layer

* Handles HTTP requests
* Calls service layer
* Returns appropriate HTTP responses

Endpoints:

* `GET /api/members`
* `GET /api/members/{id}`

---

## ⚙️ Dependency Injection

Registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IMembers, MembersRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
```

---

## 🔄 Request Flow

```
Client → Controller → Service → Repository → Response
```

---

## 🧪 Testing Setup

### Frameworks Used

* xUnit
* Moq

---

### What is Tested?

#### ✅ Service Layer Testing

* Repository is mocked using Moq
* Focus on business logic isolation

---

### Example Test (Happy Path)

```csharp
[Fact]
public async Task GetAllMembersAsync_ReturnsMemberList()
{
    var mockRepo = new Mock<IMembers>();

    mockRepo.Setup(x => x.GetAllMembers())
            .Returns(new List<Member> { new Member { MemberId = 1 } });

    var service = new MemberService(mockRepo.Object);

    var result = await service.GetAllMembersAsync();

    Assert.NotNull(result);
    Assert.Single(result);
}
```

---

### Example Test (Exception Case)

```csharp
[Fact]
public async Task GetMemberByIdAsync_WhenNotFound_ThrowsException()
{
    var mockRepo = new Mock<IMembers>();

    mockRepo.Setup(x => x.GetMember(It.IsAny<int>()))
            .Returns((Member)null);

    var service = new MemberService(mockRepo.Object);

    await Assert.ThrowsAsync<Exception>(() => service.GetMemberByIdAsync(10));
}
```

---

### Moq Concepts Used

* `Mock<T>` → Create fake dependency
* `Setup()` → Define behavior
* `Returns()` → Return mock data
* `Verify()` → Check method calls

---

## 🧠 Key Concepts Learned

* Repository Pattern
* Service Layer Abstraction
* Dependency Injection (Scoped lifetime)
* Async Programming (Task-based)
* Unit Testing with Moq
* Arrange-Act-Assert pattern

---

## ⚠️ Current Limitations

* In-memory data (no database)
* No DTO layer
* No logging
* No validation
* No global exception handling
* No authentication

---

## 🚀 Next Steps (Future Enhancements)

### Phase 2 (Testing Expansion)

* Controller testing using Moq
* Integration testing using WebApplicationFactory

### Phase 3 (Production Upgrade)

* EF Core + SQL Server
* Async repository methods
* DTO + AutoMapper
* JWT Authentication
* Logging (ILogger)
* Global Exception Middleware

---

## 🎤 Interview Summary

This project demonstrates:

* Clean separation of concerns
* Testable architecture using DI
* Unit testing with mocked dependencies
* Understanding of API request flow

---

## 📌 How to Use This Context

When continuing with AI assistant, say:

> "Use MemberApi context file and continue with [next task]"

Example:

* "continue with controller testing"
* "upgrade this to EF Core"
* "add JWT authentication"

---

## ✅ Status

✔ API created
✔ Service layer added
✔ Async conversion done
✔ Unit testing with Moq completed

➡ Next: Controller testing / Integration testing / Production upgrade

---

# 🐳 Containerization (Podman)

## 📌 Objective

Containerize the ASP.NET Core Web API using Podman with a multi-stage Dockerfile.

---

## ✅ Concepts Learned

* Difference between image and container
* Multi-stage Docker builds
* Runtime vs SDK images
* Port binding (`8080:80`)
* ENTRYPOINT usage
* Environment variables inside containers
* Container lifecycle debugging

---

## 📄 Dockerfile Used

```dockerfile
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY MemberApi.csproj ./

RUN dotnet restore

COPY . .

RUN dotnet publish MemberApi.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "MemberApi.dll"]
```

---

## 🚀 Commands Used

### Build image

```bash
podman build --no-cache -t member-api .
```

### Verify image

```bash
podman images
```

### Run container

```bash
podman run -p 8080:80 member-api
```

### View running containers

```bash
podman ps
```

### View logs

```bash
podman logs <container_id>
```

### Cleanup containers

```bash
podman rm -a
```

### Cleanup images

```bash
podman rmi -a
```

---

## ⚠️ Important Learnings

### Solution Publish Issue

Container build failed because Docker attempted to publish `.sln` which referenced test projects.

Fix:

```dockerfile
RUN dotnet publish MemberApi.csproj
```

instead of:

```dockerfile
RUN dotnet publish
```

---

### Swagger in Container

Swagger was unavailable because container runs in Production environment by default.

Fix:

```dockerfile
ENV ASPNETCORE_ENVIRONMENT=Development
```

---

### HTTPS Redirection Issue

HTTPS redirection caused issues inside container environment.

Temporary fix:

```csharp
// app.UseHttpsRedirection();
```

---

## 🎤 Interview-Level Understanding

* Multi-stage builds reduce runtime image size
* SDK image is used for build/publish
* ASP.NET runtime image is used for execution
* Containers require long-running process via ENTRYPOINT
* Images are immutable templates
* Containers are runtime instances

---

## ✅ Current Status

✔ API created
✔ Service layer added
✔ Async conversion done
✔ Unit testing with Moq completed
✔ Containerized using Podman

