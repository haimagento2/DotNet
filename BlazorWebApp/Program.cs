using System.Data.Common;
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

builder.Services.AddControllers();

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
// builder.Services.AddHostedService<BlazorWebApp.Services.LicenseExpirationHostedService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var seedDataPath = Path.Combine(builder.Environment.ContentRootPath, "SeedData");
    var importCsvData = args.Contains("--import-csv", StringComparer.OrdinalIgnoreCase);

    dbContext.Database.EnsureCreated();
    EnsureProductsTables(dbContext);

    if (importCsvData || !dbContext.CustomerGroups.Any())
    {
        CsvSeedImporter.ImportSeedData(dbContext, seedDataPath, force: importCsvData);
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

static void EnsureProductsTables(ApplicationDbContext dbContext)
{
    if (TableExists(dbContext, "Products") && !ColumnExists(dbContext, "Products", "product_id") && ColumnExists(dbContext, "Products", "Id"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Products RENAME COLUMN Id TO product_id;");
    }

    if (TableExists(dbContext, "ProductCategories") && !ColumnExists(dbContext, "ProductCategories", "category_id") && ColumnExists(dbContext, "ProductCategories", "Id"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProductCategories RENAME COLUMN Id TO category_id;");
    }

    dbContext.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS Products (
            product_id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            sku TEXT NOT NULL,
            type TEXT NOT NULL,
            price REAL NOT NULL DEFAULT 0,
            image TEXT NOT NULL,
            description TEXT NOT NULL,
            ProductCategoryId INTEGER,
            Period TEXT NOT NULL,
            EnableSubscriptionLinking INTEGER NOT NULL DEFAULT 0,
            LicenseOnly INTEGER NOT NULL DEFAULT 0,
            CommunityOnly INTEGER NOT NULL DEFAULT 0,
            SendEmailInstruction INTEGER NOT NULL DEFAULT 0
        );

        CREATE TABLE IF NOT EXISTS ProductCategories (
            category_id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            parent_id INTEGER,
            image TEXT NOT NULL,
            description TEXT NOT NULL,
            CreatedAt TEXT NOT NULL,
            UpdatedAt TEXT,
            FOREIGN KEY (parent_id) REFERENCES ProductCategories (category_id) ON DELETE SET NULL
        );

        CREATE TABLE IF NOT EXISTS category_product_links (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            category_id INTEGER NOT NULL,
            product_id INTEGER NOT NULL,
            FOREIGN KEY (category_id) REFERENCES ProductCategories (category_id) ON DELETE CASCADE,
            FOREIGN KEY (product_id) REFERENCES Products (product_id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS ProgramProductAssociated (
            ProgramId INTEGER NOT NULL,
            ProductId INTEGER NOT NULL,
            type TEXT,
            period TEXT,
            custom_period INTEGER,
            linking_license INTEGER,
            community_only INTEGER,
            auto_renew_license INTEGER NOT NULL DEFAULT 0,
            send_email_instruction INTEGER NOT NULL DEFAULT 0,
            PRIMARY KEY (ProgramId, ProductId),
            FOREIGN KEY (ProgramId) REFERENCES Programs (Id) ON DELETE CASCADE,
            FOREIGN KEY (ProductId) REFERENCES Products (product_id) ON DELETE CASCADE
        );

        CREATE INDEX IF NOT EXISTS IX_ProgramProductAssociated_ProductId ON ProgramProductAssociated (ProductId);
        CREATE INDEX IF NOT EXISTS IX_category_product_links_category_id ON category_product_links (category_id);
        CREATE INDEX IF NOT EXISTS IX_category_product_links_product_id ON category_product_links (product_id);
    ");

    if (!ColumnExists(dbContext, "Products", "type"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Products ADD COLUMN type TEXT NOT NULL DEFAULT '';");
    }
    if (!ColumnExists(dbContext, "Products", "price"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Products ADD COLUMN price REAL NOT NULL DEFAULT 0;");
    }
    if (!ColumnExists(dbContext, "Products", "image"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Products ADD COLUMN image TEXT NOT NULL DEFAULT '';");
    }
    if (!ColumnExists(dbContext, "Products", "description"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Products ADD COLUMN description TEXT NOT NULL DEFAULT '';");
    }
    if (!ColumnExists(dbContext, "ProductCategories", "parent_id"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProductCategories ADD COLUMN parent_id INTEGER;");
    }
    if (!ColumnExists(dbContext, "ProductCategories", "image"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProductCategories ADD COLUMN image TEXT NOT NULL DEFAULT '';");
    }
    if (!ColumnExists(dbContext, "ProductCategories", "description"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProductCategories ADD COLUMN description TEXT NOT NULL DEFAULT '';");
    }
    if (TableExists(dbContext, "ProgramProductAssociated") && !ColumnExists(dbContext, "ProgramProductAssociated", "custom_period"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProgramProductAssociated ADD COLUMN custom_period INTEGER;");
    }
    if (TableExists(dbContext, "ProgramProductAssociated") && !ColumnExists(dbContext, "ProgramProductAssociated", "auto_renew_license"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProgramProductAssociated ADD COLUMN auto_renew_license INTEGER NOT NULL DEFAULT 0;");
    }
    if (TableExists(dbContext, "ProgramProductAssociated") && !ColumnExists(dbContext, "ProgramProductAssociated", "send_email_instruction"))
    {
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE ProgramProductAssociated ADD COLUMN send_email_instruction INTEGER NOT NULL DEFAULT 0;");
    }
}

static bool TableExists(ApplicationDbContext dbContext, string tableName)
{
    var connection = dbContext.Database.GetDbConnection();
    if (connection.State != System.Data.ConnectionState.Open)
    {
        connection.Open();
    }

    using var command = connection.CreateCommand();
    command.CommandText = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}';";
    return Convert.ToInt32(command.ExecuteScalar() ?? 0) > 0;
}

static bool ColumnExists(ApplicationDbContext dbContext, string tableName, string columnName)
{
    var connection = dbContext.Database.GetDbConnection();
    if (connection.State != System.Data.ConnectionState.Open)
    {
        connection.Open();
    }

    using var command = connection.CreateCommand();
    command.CommandText = $"PRAGMA table_info('{tableName}');";

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
    }

    return false;
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
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
