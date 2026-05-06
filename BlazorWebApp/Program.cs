using System.IO;
using BlazorWebApp.Components;
using BlazorWebApp.Data;
using BlazorWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var truncateLicenseData = args.Contains("--truncate-licenses", StringComparer.OrdinalIgnoreCase);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

var databasePath = Path.Combine(builder.Environment.ContentRootPath, "BlazorWebApp.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<BlazorWebApp.Services.AuthenticationService>();
builder.Services.AddScoped<BlazorWebApp.Services.LicenseService>();
builder.Services.AddHostedService<BlazorWebApp.Services.LicenseExpirationHostedService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureDeleted();
    }

    dbContext.Database.EnsureCreated();

    if (app.Environment.IsDevelopment() || truncateLicenseData)
    {
        ResetLicenseData(dbContext);
    }

    if (!dbContext.Admins.Any())
    {
        dbContext.Admins.Add(new Admin
        {
            Name = "admin",
            Email = "admin@gmail.com",
            PasswordHash = Admin.HashPassword("admin123"),
            CreatedAt = new DateTime(2026, 1, 1)
        });
        dbContext.SaveChanges();
    }

    if (truncateLicenseData)
    {
        Console.WriteLine("Truncated Licenses and LicenseMembers data.");
        return;
    }
}

static void ResetLicenseData(ApplicationDbContext dbContext)
{
    dbContext.Database.ExecuteSqlRaw("DELETE FROM LicenseMembers;");
    dbContext.Database.ExecuteSqlRaw("DELETE FROM Licenses;");
    dbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name IN ('LicenseMembers', 'Licenses');");
    dbContext.SaveChanges();

    var licenses = new List<License>
    {
        new License { Id = 1,  LicenseKey = 1001, ProgramId = 1,  CompanyId = 1,  CustomerGroupId = null, OwnerId = 1,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,1,1),  ExpiryDate = new DateTime(2026,1,1),  MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025,1,1) },
        new License { Id = 2,  LicenseKey = 1002, ProgramId = 2,  CompanyId = 2,  CustomerGroupId = null, OwnerId = 2,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,2,1),  ExpiryDate = new DateTime(2026,2,1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025,2,1) },
        new License { Id = 3,  LicenseKey = 1003, ProgramId = 3,  CompanyId = 3,  CustomerGroupId = null, OwnerId = 3,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,3,1),  ExpiryDate = new DateTime(2026,3,1),  MaxMembers = 50, Status = "Active",    CreatedAt = new DateTime(2025,3,1) },
        new License { Id = 4,  LicenseKey = 1004, ProgramId = 4,  CompanyId = 4,  CustomerGroupId = null, OwnerId = 4,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,4,1),  ExpiryDate = new DateTime(2026,4,1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025,4,1) },
        new License { Id = 5,  LicenseKey = 1005, ProgramId = 5,  CompanyId = 5,  CustomerGroupId = null, OwnerId = 5,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,5,1),  ExpiryDate = new DateTime(2026,5,1),  MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025,5,1) },
        new License { Id = 6,  LicenseKey = 1006, ProgramId = 1,  CompanyId = null, CustomerGroupId = 1,  OwnerId = 1,  SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025,6,1),  ExpiryDate = new DateTime(2026,6,1),  MaxMembers = 15, Status = "Active",    CreatedAt = new DateTime(2025,6,1) },
        new License { Id = 7,  LicenseKey = 1007, ProgramId = 2,  CompanyId = null, CustomerGroupId = 2,  OwnerId = 2,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,7,1),  ExpiryDate = new DateTime(2026,7,1),  MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025,7,1) },
        new License { Id = 8,  LicenseKey = 1008, ProgramId = 3,  CompanyId = 8,  CustomerGroupId = null, OwnerId = 8,  SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025,1,15), ExpiryDate = new DateTime(2026,1,15), MaxMembers = 25, Status = "Suspended", CreatedAt = new DateTime(2025,1,15) },
        new License { Id = 9,  LicenseKey = 1009, ProgramId = 4,  CompanyId = null, CustomerGroupId = 3,  OwnerId = 9,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,2,15), ExpiryDate = new DateTime(2026,2,15), MaxMembers = 30, Status = "Active",    CreatedAt = new DateTime(2025,2,15) },
        new License { Id = 10, LicenseKey = 1010, ProgramId = 5,  CompanyId = 10, CustomerGroupId = null, OwnerId = 10, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,3,15), ExpiryDate = new DateTime(2026,3,15), MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025,3,15) },
        new License { Id = 11, LicenseKey = 1011, ProgramId = 1,  CompanyId = null, CustomerGroupId = 4,  OwnerId = 11, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,4,15), ExpiryDate = new DateTime(2025,10,15), MaxMembers = 15, Status = "Expired",   CreatedAt = new DateTime(2025,4,15) },
        new License { Id = 12, LicenseKey = 1012, ProgramId = 2,  CompanyId = 12, CustomerGroupId = null, OwnerId = 12, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,5,15), ExpiryDate = new DateTime(2026,5,15), MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025,5,15) },
        new License { Id = 13, LicenseKey = 1013, ProgramId = 3,  CompanyId = null, CustomerGroupId = 5,  OwnerId = 13, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025,6,15), ExpiryDate = new DateTime(2026,6,15), MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025,6,15) },
        new License { Id = 14, LicenseKey = 1014, ProgramId = 4,  CompanyId = 14, CustomerGroupId = null, OwnerId = 14, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,7,15), ExpiryDate = new DateTime(2026,7,15), MaxMembers = 10, Status = "Suspended", CreatedAt = new DateTime(2025,7,15) },
        new License { Id = 15, LicenseKey = 1015, ProgramId = 5,  CompanyId = 15, CustomerGroupId = null, OwnerId = 15, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,8,15), ExpiryDate = new DateTime(2026,8,15), MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025,8,15) },
        new License { Id = 16, LicenseKey = 1016, ProgramId = 1,  CompanyId = null, CustomerGroupId = 1,  OwnerId = 16, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,9,15), ExpiryDate = new DateTime(2026,9,15), MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025,9,15) },
        new License { Id = 17, LicenseKey = 1017, ProgramId = 2,  CompanyId = 17, CustomerGroupId = null, OwnerId = 17, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025,10,15), ExpiryDate = new DateTime(2026,10,15), MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025,10,15) },
        new License { Id = 18, LicenseKey = 1018, ProgramId = 3,  CompanyId = null, CustomerGroupId = 2,  OwnerId = 18, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025,11,15), ExpiryDate = new DateTime(2026,11,15), MaxMembers = 15, Status = "Active",    CreatedAt = new DateTime(2025,11,15) },
        new License { Id = 19, LicenseKey = 1019, ProgramId = 4,  CompanyId = 19, CustomerGroupId = null, OwnerId = 19, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025,12,15), ExpiryDate = new DateTime(2026,12,15), MaxMembers = 25, Status = "Active",    CreatedAt = new DateTime(2025,12,15) },
        new License { Id = 20, LicenseKey = 1020, ProgramId = 5,  CompanyId = null, CustomerGroupId = 3,  OwnerId = 20, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2026,1,15),  ExpiryDate = new DateTime(2027,1,15),  MaxMembers = 5,  Status = "Expired",   CreatedAt = new DateTime(2026,1,15) },
        new License { Id = 21, LicenseKey = 1021, ProgramId = 1,  CompanyId = 2,  CustomerGroupId = null, OwnerId = 1,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2026,2,1),  ExpiryDate = new DateTime(2027,2,1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2026,2,1) },
        new License { Id = 22, LicenseKey = 1022, ProgramId = 2,  CompanyId = 4,  CustomerGroupId = null, OwnerId = 2,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2026,3,1),  ExpiryDate = new DateTime(2027,3,1),  MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2026,3,1) }
    };

    dbContext.Licenses.AddRange(licenses);
    dbContext.SaveChanges();

    var members = new List<LicenseMember>
    {
        new LicenseMember { Id = 1,  LicenseId = 1,  CustomerId = 2,  OwnerId = 1,  AssignedAt = new DateTime(2025,1,10), CreatedAt = new DateTime(2025,1,10) },
        new LicenseMember { Id = 2,  LicenseId = 1,  CustomerId = 3,  OwnerId = 1,  AssignedAt = new DateTime(2025,1,12), CreatedAt = new DateTime(2025,1,12) },
        new LicenseMember { Id = 3,  LicenseId = 2,  CustomerId = 4,  OwnerId = 2,  AssignedAt = new DateTime(2025,2,5),  CreatedAt = new DateTime(2025,2,5) },
        new LicenseMember { Id = 4,  LicenseId = 2,  CustomerId = 5,  OwnerId = 2,  AssignedAt = new DateTime(2025,2,8),  CreatedAt = new DateTime(2025,2,8) },
        new LicenseMember { Id = 5,  LicenseId = 3,  CustomerId = 6,  OwnerId = 3,  AssignedAt = new DateTime(2025,3,5),  CreatedAt = new DateTime(2025,3,5) },
        new LicenseMember { Id = 6,  LicenseId = 3,  CustomerId = 7,  OwnerId = 3,  AssignedAt = new DateTime(2025,3,6),  CreatedAt = new DateTime(2025,3,6) },
        new LicenseMember { Id = 7,  LicenseId = 3,  CustomerId = 8,  OwnerId = 3,  AssignedAt = new DateTime(2025,3,7),  CreatedAt = new DateTime(2025,3,7) },
        new LicenseMember { Id = 8,  LicenseId = 4,  CustomerId = 9,  OwnerId = 4,  AssignedAt = new DateTime(2025,4,3),  CreatedAt = new DateTime(2025,4,3) },
        new LicenseMember { Id = 9,  LicenseId = 5,  CustomerId = 10, OwnerId = 5,  AssignedAt = new DateTime(2025,5,2),  CreatedAt = new DateTime(2025,5,2) },
        new LicenseMember { Id = 10, LicenseId = 5,  CustomerId = 11, OwnerId = 5,  AssignedAt = new DateTime(2025,5,4),  CreatedAt = new DateTime(2025,5,4) },
        new LicenseMember { Id = 11, LicenseId = 6,  CustomerId = 12, OwnerId = 1,  AssignedAt = new DateTime(2025,6,2),  CreatedAt = new DateTime(2025,6,2) },
        new LicenseMember { Id = 12, LicenseId = 7,  CustomerId = 13, OwnerId = 2,  AssignedAt = new DateTime(2025,6,5),  CreatedAt = new DateTime(2025,6,5) },
        new LicenseMember { Id = 13, LicenseId = 7,  CustomerId = 14, OwnerId = 2,  AssignedAt = new DateTime(2025,6,7),  CreatedAt = new DateTime(2025,6,7) },
        new LicenseMember { Id = 14, LicenseId = 8,  CustomerId = 15, OwnerId = 8,  AssignedAt = new DateTime(2025,1,20), CreatedAt = new DateTime(2025,1,20) },
        new LicenseMember { Id = 15, LicenseId = 8,  CustomerId = 16, OwnerId = 8,  AssignedAt = new DateTime(2025,1,22), CreatedAt = new DateTime(2025,1,22) },
        new LicenseMember { Id = 16, LicenseId = 9,  CustomerId = 17, OwnerId = 9,  AssignedAt = new DateTime(2025,2,20), CreatedAt = new DateTime(2025,2,20) },
        new LicenseMember { Id = 17, LicenseId = 10, CustomerId = 18, OwnerId = 10, AssignedAt = new DateTime(2025,3,20), CreatedAt = new DateTime(2025,3,20) },
        new LicenseMember { Id = 18, LicenseId = 10, CustomerId = 19, OwnerId = 10, AssignedAt = new DateTime(2025,3,22), CreatedAt = new DateTime(2025,3,22) },
        new LicenseMember { Id = 19, LicenseId = 11, CustomerId = 20, OwnerId = 11, AssignedAt = new DateTime(2025,4,20), CreatedAt = new DateTime(2025,4,20) },
        new LicenseMember { Id = 20, LicenseId = 12, CustomerId = 1,  OwnerId = 12, AssignedAt = new DateTime(2025,5,20), CreatedAt = new DateTime(2025,5,20) },
        new LicenseMember { Id = 21, LicenseId = 13, CustomerId = 2,  OwnerId = 13, AssignedAt = new DateTime(2025,6,20), CreatedAt = new DateTime(2025,6,20) },
        new LicenseMember { Id = 22, LicenseId = 14, CustomerId = 3,  OwnerId = 14, AssignedAt = new DateTime(2025,7,20), CreatedAt = new DateTime(2025,7,20) },
        new LicenseMember { Id = 23, LicenseId = 15, CustomerId = 4,  OwnerId = 15, AssignedAt = new DateTime(2025,8,20), CreatedAt = new DateTime(2025,8,20) },
        new LicenseMember { Id = 24, LicenseId = 16, CustomerId = 5,  OwnerId = 16, AssignedAt = new DateTime(2025,9,20), CreatedAt = new DateTime(2025,9,20) },
        new LicenseMember { Id = 25, LicenseId = 17, CustomerId = 6,  OwnerId = 17, AssignedAt = new DateTime(2025,10,20),CreatedAt = new DateTime(2025,10,20) },
        new LicenseMember { Id = 26, LicenseId = 18, CustomerId = 7,  OwnerId = 18, AssignedAt = new DateTime(2025,11,20),CreatedAt = new DateTime(2025,11,20) },
        new LicenseMember { Id = 27, LicenseId = 19, CustomerId = 8,  OwnerId = 19, AssignedAt = new DateTime(2025,12,20),CreatedAt = new DateTime(2025,12,20) },
        new LicenseMember { Id = 28, LicenseId = 20, CustomerId = 9,  OwnerId = 20, AssignedAt = new DateTime(2026,1,20), CreatedAt = new DateTime(2026,1,20) },
        new LicenseMember { Id = 29, LicenseId = 21, CustomerId = 10, OwnerId = 1,  AssignedAt = new DateTime(2026,2,10), CreatedAt = new DateTime(2026,2,10) },
        new LicenseMember { Id = 30, LicenseId = 22, CustomerId = 11, OwnerId = 2,  AssignedAt = new DateTime(2026,3,10), CreatedAt = new DateTime(2026,3,10) }
    };

    dbContext.LicenseMembers.AddRange(members);
    dbContext.SaveChanges();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (HttpMethods.IsHead(context.Request.Method))
    {
        context.Request.Method = HttpMethods.Get;
    }

    await next();
});

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
