using Microsoft.Data.Sqlite;
using System.Data;

var dbPath = Path.Combine("BlazorWebApp", "BlazorWebApp.db");
if (!File.Exists(dbPath))
{
    Console.Error.WriteLine($"Database not found: {dbPath}");
    return 1;
}

var connString = new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString();
using var conn = new SqliteConnection(connString);
conn.Open();

using var countCmd = conn.CreateCommand();
countCmd.CommandText = "SELECT COUNT(*) FROM Customers;";
var count = Convert.ToInt32(countCmd.ExecuteScalar() ?? 0);
Console.WriteLine($"Customers: {count}");

string? arg0 = args.Length > 0 ? args[0] : null;
if (arg0 == "--find" && args.Length > 1)
{
    var term = args[1];
    if (int.TryParse(term, out var id))
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, Email, City FROM Customers WHERE Id = $id LIMIT 1;";
        cmd.Parameters.AddWithValue("$id", id);
        using var r = cmd.ExecuteReader();
        if (r.Read())
        {
            Console.WriteLine($"{r.GetInt32(0)} | {r.GetString(1)} {r.GetString(2)} | {r.GetString(3)} | {r.GetString(4)}");
        }
        else
        {
            Console.WriteLine($"Customer with Id={id} not found.");
        }
    }
    else
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, Email, City FROM Customers WHERE FirstName LIKE $q OR LastName LIKE $q OR Email LIKE $q LIMIT 50;";
        cmd.Parameters.AddWithValue("$q", $"%{term}%");
        using var r = cmd.ExecuteReader();
        var found = false;
        while (r.Read())
        {
            found = true;
            Console.WriteLine($"{r.GetInt32(0)} | {r.GetString(1)} {r.GetString(2)} | {r.GetString(3)} | {r.GetString(4)}");
        }
        if (!found) Console.WriteLine($"No customers match '{term}'.");
    }
}
else
{
    using var sampleCmd = conn.CreateCommand();
    sampleCmd.CommandText = "SELECT Id, FirstName, LastName, Email, City FROM Customers ORDER BY Id DESC LIMIT 10;";
    using var reader = sampleCmd.ExecuteReader();
    Console.WriteLine("Latest rows:");
    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} {reader.GetString(2)} | {reader.GetString(3)} | {reader.GetString(4)}");
    }
}

return 0;