using MemberApi.Interfaces;
using MemberApi.Repositories;
using MemberApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMembers, MembersRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

// ✅ Enable Swagger ALWAYS (for now)
app.UseSwagger();
app.UseSwaggerUI();

// ❌ Disable HTTPS in container
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();