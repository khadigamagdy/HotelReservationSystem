using HotelReservationSystem.Data;
using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Repositories
// =========================

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// =========================
// Services
// =========================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICheckInOutService, CheckInOutService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();

// =========================
// Password Hashing
// =========================

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// =========================
// Authentication
// =========================

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// =========================
// Authorization
// =========================

builder.Services.AddAuthorization();

var app = builder.Build();

// =========================
// Create Default Manager
// =========================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<HotelDbContext>();

    var manager = context.Users
        .FirstOrDefault(user => user.Role == UserRole.Manager);

    if (manager == null)
    {
        var passwordHasher = new PasswordHasher<User>();

        manager = new User
        {
            FullName = "Hotel Manager",
            Email = "manager@hotel.com",
            Role = UserRole.Manager,
            CreatedAt = DateTime.UtcNow
        };

        manager.PasswordHash = passwordHasher.HashPassword(
            manager,
            "Manager@123"
        );

        context.Users.Add(manager);
        context.SaveChanges();
    }

    // =========================
    // Seed Room Types
    // =========================

    var singleRoomType = context.RoomTypes
        .FirstOrDefault(roomType => roomType.Name == "Single");

    if (singleRoomType == null)
    {
        singleRoomType = new RoomType
        {
            Name = "Single",
            BasePricePerNight = 1000,
            Capacity = 1,
            Description = "Comfortable room suitable for one guest."
        };

        context.RoomTypes.Add(singleRoomType);
    }

    var doubleRoomType = context.RoomTypes
        .FirstOrDefault(roomType => roomType.Name == "Double");

    if (doubleRoomType == null)
    {
        doubleRoomType = new RoomType
        {
            Name = "Double",
            BasePricePerNight = 1500,
            Capacity = 2,
            Description = "Comfortable room suitable for two guests."
        };

        context.RoomTypes.Add(doubleRoomType);
    }

    var suiteRoomType = context.RoomTypes
        .FirstOrDefault(roomType => roomType.Name == "Suite");

    if (suiteRoomType == null)
    {
        suiteRoomType = new RoomType
        {
            Name = "Suite",
            BasePricePerNight = 2500,
            Capacity = 4,
            Description = "Spacious suite suitable for families or groups."
        };

        context.RoomTypes.Add(suiteRoomType);
    }

    context.SaveChanges();

    // =========================
    // Seed Rooms
    // =========================

    if (!context.Rooms.Any(room => room.RoomNumber == "101"))
    {
        context.Rooms.Add(new Room
        {
            RoomNumber = "101",
            FloorNumber = 1,
            Status = RoomStatus.Available,
            RoomTypeId = singleRoomType.Id,
            CreatedByUserId = manager.Id
        });
    }

    if (!context.Rooms.Any(room => room.RoomNumber == "102"))
    {
        context.Rooms.Add(new Room
        {
            RoomNumber = "102",
            FloorNumber = 1,
            Status = RoomStatus.Available,
            RoomTypeId = singleRoomType.Id,
            CreatedByUserId = manager.Id
        });
    }

    if (!context.Rooms.Any(room => room.RoomNumber == "201"))
    {
        context.Rooms.Add(new Room
        {
            RoomNumber = "201",
            FloorNumber = 2,
            Status = RoomStatus.Available,
            RoomTypeId = doubleRoomType.Id,
            CreatedByUserId = manager.Id
        });
    }

    if (!context.Rooms.Any(room => room.RoomNumber == "202"))
    {
        context.Rooms.Add(new Room
        {
            RoomNumber = "202",
            FloorNumber = 2,
            Status = RoomStatus.Available,
            RoomTypeId = doubleRoomType.Id,
            CreatedByUserId = manager.Id
        });
    }

    if (!context.Rooms.Any(room => room.RoomNumber == "301"))
    {
        context.Rooms.Add(new Room
        {
            RoomNumber = "301",
            FloorNumber = 3,
            Status = RoomStatus.Available,
            RoomTypeId = suiteRoomType.Id,
            CreatedByUserId = manager.Id
        });
    }

    context.SaveChanges();
}

// =========================
// HTTP Request Pipeline
// =========================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();