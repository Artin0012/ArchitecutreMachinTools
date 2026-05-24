using _1.Domain.Core.Extentions;
using _2.Application.Context;
using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using _3.Infrastructure.Repository;
using _3.Infrastructure.Services;
using ArchitecutreMachin.Profiles;
using ArchitecutreMachins.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;
//using ArchitecutreMachin.Features.Cars.Commands;,

// SeriLog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
    "Logs/log-.txt",
    rollingInterval: RollingInterval.Day)
    .CreateLogger();

// WebApplication
var builder = WebApplication.CreateBuilder(args);

// cfg Serilog
builder.Host.UseSerilog();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("2.Application")
    ));

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Repository Registration
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarFeatureRepository, CarFeatureRepository>();

////MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        Assembly.Load("ArchitecutreMachins"));
});

//Register FluentValidation
builder.Services.AddValidatorsFromAssembly(
    Assembly.Load("ArchitecutreMachins"));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Controllers
builder.Services.AddControllers();

// Fluent
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();

// JWT
builder.Services.AddScoped<JwtService>();

//Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"]!)
        ),
        RoleClaimType = ClaimTypes.Role
    };
});


// swagger security
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();

var app = builder.Build();

//Premission Run
async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    // تعریف Roles
    string[] roles = { "Admin", "User", "Editor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    // ساخت پیش فرض Admin
    string adminUserName = "Admin";
    string adminEmail = "Admin@gmail.com";
    string adminPassword = "A123456789z";

    var adminUser = await userManager.FindByNameAsync(adminUserName);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminUserName,
            Email = adminEmail,
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}


// Seed فراخوانی
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdminAsync(services);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
