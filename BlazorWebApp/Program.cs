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

    var seedDataPath = Path.Combine(builder.Environment.ContentRootPath, "SeedData");
    var importCsvData = args.Contains("--import-csv", StringComparer.OrdinalIgnoreCase);

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureDeleted();
    }

    dbContext.Database.EnsureCreated();

    if (app.Environment.IsDevelopment() || importCsvData || !dbContext.CustomerGroups.Any())
    {
        CsvSeedImporter.ImportSeedData(dbContext, seedDataPath, force: app.Environment.IsDevelopment() || importCsvData);
    }

    if (truncateLicenseData)
    {
        ResetLicenseData(dbContext);
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
