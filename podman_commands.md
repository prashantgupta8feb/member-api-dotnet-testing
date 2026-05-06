# 🐳 ASP.NET Core Web API Containerization Guide (Podman)

## 📌 Goal

Containerize the ASP.NET Core Web API project using Podman and understand the purpose behind each step.

---

# 🧠 Core Concepts

## 🔹 Image

A blueprint/template containing:

* Application code
* Runtime
* Dependencies

Example:

```plaintext
member-api image
```

---

## 🔹 Container

A running instance of an image.

Example:

```plaintext
member-api container
```

---

## 🔹 Dockerfile

Instruction file used to build container images.

Think of it as:

```plaintext
Recipe for building container image
```

---

# 🧹 Cleanup Steps (Important Before Fresh Build)

## 1. View all containers

```bash
podman ps -a
```

### Why?

Shows:

* Running containers
* Stopped containers

---

## 2. Remove all containers

```bash
podman rm -a
```

### Why?

Removes old/broken runtime instances.

---

## 3. View images

```bash
podman images
```

---

## 4. Remove all images

```bash
podman rmi -a
```

### Why?

Removes:

* Cached layers
* Broken images
* Wrong entrypoint configurations

---

## 5. Remove unused cache/resources

```bash
podman system prune -a
```

### Why?

Cleans:

* Unused layers
* Networks
* Build cache

---

# 📄 Dockerfile

Create a file named:

```plaintext
Dockerfile
```

inside the API project folder (same level as `.csproj`)

---

## ✅ Correct Dockerfile (.NET 10)

```dockerfile
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy only project file first
COPY MemberApi.csproj ./

RUN dotnet restore

# Copy remaining files
COPY . .

# Publish API project
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

# 🧠 Dockerfile Explanation

## 🔹 Build Image

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0
```

Used for:

* Restoring packages
* Building application
* Publishing application

---

## 🔹 Runtime Image

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
```

Smaller image used only to run the app.

---

## 🔹 WORKDIR

```dockerfile
WORKDIR /src
```

Equivalent to:

```bash
cd /src
```

---

## 🔹 COPY

```dockerfile
COPY . .
```

Copies application files into container.

---

## 🔹 dotnet restore

```dockerfile
RUN dotnet restore
```

Downloads NuGet packages.

---

## 🔹 dotnet publish

```dockerfile
RUN dotnet publish MemberApi.csproj -c Release -o /app/publish
```

Creates optimized deployable output.

---

## 🔹 ASPNETCORE_URLS

```dockerfile
ENV ASPNETCORE_URLS=http://+:80
```

Makes application listen on:

```plaintext
Port 80 inside container
```

---

## 🔹 ENTRYPOINT

```dockerfile
ENTRYPOINT ["dotnet", "MemberApi.dll"]
```

Main process that keeps container alive.

---

# ⚠️ Important Program.cs Change

Inside container, temporarily disable HTTPS redirection.

Comment this:

```csharp
// app.UseHttpsRedirection();
```

---

# 🚀 Build Container Image

Run inside project folder:

```bash
podman build --no-cache -t member-api .
```

---

# 🧠 What Happens Internally?

Podman:

1. Reads Dockerfile
2. Pulls base images
3. Copies code
4. Builds application
5. Creates final image

---

# 🔍 Verify Image

```bash
podman images
```

Expected:

```plaintext
localhost/member-api
```

---

# ▶️ Run Container

```bash
podman run -p 8080:80 member-api
```

---

# 🌐 Access Swagger

Open:

```plaintext
http://localhost:8080/swagger
```

---

# 🧪 Verify Running Container

```bash
podman ps
```

---

# 📜 View Container Logs

```bash
podman logs <container_id>
```

---

# ⚠️ Common Issues Learned

## ❌ Dangling Image (`<none>`)

Cause:

* Build succeeded partially
* Final image tagging failed

---

## ❌ Solution File Publish Error

Cause:

* Docker tried publishing `.sln`
* Test project missing inside container

Fix:

```dockerfile
RUN dotnet publish MemberApi.csproj
```

instead of:

```dockerfile
RUN dotnet publish
```

---

## ❌ Container Exits Immediately

Possible causes:

* Wrong ENTRYPOINT
* No running process
* Wrong DLL name
* App not listening on correct port

---

# 🎤 Interview-Level Talking Points

## What is containerization?

Packaging application + dependencies into portable runtime unit.

---

## Why multi-stage build?

* Smaller runtime image
* Better security
* Faster deployment

---

## Why publish `.csproj` instead of `.sln`?

Only deployable project should be containerized. Test projects are unnecessary in runtime containers.

---

## Difference between Image and Container?

| Image     | Container        |
| --------- | ---------------- |
| Blueprint | Running instance |
| Static    | Runtime          |

---

# ✅ Final Achievement

✔ ASP.NET Core Web API created
✔ Service layer added
✔ Unit testing with Moq completed
✔ Containerized using Podman
✔ Understood image/container lifecycle
✔ Learned real-world container debugging
