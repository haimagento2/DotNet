using System.Globalization;
using BlazorWebApp.Models;

namespace BlazorWebApp.Data
{
    public static class CsvSeedImporter
    {
        public static void ImportSeedData(ApplicationDbContext dbContext, string seedFolder, bool force = false)
        {
            if (!Directory.Exists(seedFolder))
            {
                throw new DirectoryNotFoundException($"Seed folder not found: {seedFolder}");
            }

            if (force || !dbContext.CustomerGroups.Any())
            {
                dbContext.CustomerGroups.AddRange(ReadCustomerGroups(Path.Combine(seedFolder, "CustomerGroups.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.Companies.Any())
            {
                dbContext.Companies.AddRange(ReadCompanies(Path.Combine(seedFolder, "Companies.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.Programs.Any())
            {
                dbContext.Programs.AddRange(ReadPrograms(Path.Combine(seedFolder, "Programs.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.Customers.Any())
            {
                dbContext.Customers.AddRange(ReadCustomers(Path.Combine(seedFolder, "Customers.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.Licenses.Any())
            {
                dbContext.Licenses.AddRange(ReadLicenses(Path.Combine(seedFolder, "Licenses.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.LicenseMembers.Any())
            {
                dbContext.LicenseMembers.AddRange(ReadLicenseMembers(Path.Combine(seedFolder, "LicenseMembers.csv")));
                dbContext.SaveChanges();
            }

            if (force || !dbContext.Admins.Any())
            {
                dbContext.Admins.AddRange(ReadAdmins(Path.Combine(seedFolder, "Admins.csv")));
                dbContext.SaveChanges();
            }
        }

        private static IEnumerable<CustomerGroup> ReadCustomerGroups(string filePath) =>
            ReadEntities(filePath, row => new CustomerGroup
            {
                Id = ParseInt(row["Id"]),
                Name = ParseString(row["Name"]),
                Description = ParseString(row["Description"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<Company> ReadCompanies(string filePath) =>
            ReadEntities(filePath, row => new Company
            {
                Id = ParseInt(row["Id"]),
                Name = ParseString(row["Name"]),
                Industry = ParseString(row["Industry"]),
                City = ParseString(row["City"]),
                Phone = ParseString(row["Phone"]),
                Email = ParseString(row["Email"]),
                Website = ParseString(row["Website"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<Customer> ReadCustomers(string filePath) =>
            ReadEntities(filePath, row => new Customer
            {
                Id = ParseInt(row["Id"]),
                FirstName = ParseString(row["FirstName"]),
                LastName = ParseString(row["LastName"]),
                Email = ParseString(row["Email"]),
                Phone = ParseString(row["Phone"]),
                City = ParseString(row["City"]),
                CompanyId = ParseNullableInt(row["CompanyId"]),
                CustomerGroupId = ParseNullableInt(row["CustomerGroupId"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<AppProgram> ReadPrograms(string filePath) =>
            ReadEntities(filePath, row => new AppProgram
            {
                Id = ParseInt(row["Id"]),
                Name = ParseString(row["Name"]),
                Description = ParseString(row["Description"]),
                Version = ParseString(row["Version"]),
                Price = ParseDecimal(row["Price"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<License> ReadLicenses(string filePath) =>
            ReadEntities(filePath, row => new License
            {
                Id = ParseInt(row["Id"]),
                LicenseKey = ParseInt(row["LicenseKey"]),
                ProgramId = ParseInt(row["ProgramId"]),
                CompanyId = ParseNullableInt(row["CompanyId"]),
                CustomerGroupId = ParseNullableInt(row["CustomerGroupId"]),
                OwnerId = ParseNullableInt(row["OwnerId"]),
                SubLicenseType = ParseLicenseType(row["SubLicenseType"]),
                StartDate = ParseDateTime(row["StartDate"]),
                ExpiryDate = ParseDateTime(row["ExpiryDate"]),
                MaxMembers = ParseInt(row["MaxMembers"]),
                Status = ParseString(row["Status"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<LicenseMember> ReadLicenseMembers(string filePath) =>
            ReadEntities(filePath, row => new LicenseMember
            {
                Id = ParseInt(row["Id"]),
                LicenseId = ParseInt(row["LicenseId"]),
                CustomerId = ParseInt(row["CustomerId"]),
                OwnerId = ParseNullableInt(row["OwnerId"]),
                Permission = ParseInt(row["Permission"]),
                AssignedAt = ParseDateTime(row["AssignedAt"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<Admin> ReadAdmins(string filePath) =>
            ReadEntities(filePath, row => new Admin
            {
                Id = ParseInt(row["Id"]),
                Name = ParseString(row["Name"]),
                Email = ParseString(row["Email"]),
                PasswordHash = ParseString(row["PasswordHash"]),
                CreatedAt = ParseDateTime(row["CreatedAt"]),
                UpdatedAt = ParseNullableDateTime(row["UpdatedAt"])
            });

        private static IEnumerable<T> ReadEntities<T>(string filePath, Func<Dictionary<string, string>, T> factory)
        {
            foreach (var row in ReadCsvRows(filePath))
            {
                yield return factory(row);
            }
        }

        private static IEnumerable<Dictionary<string, string>> ReadCsvRows(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"CSV seed file not found: {filePath}", filePath);
            }

            using var reader = new StreamReader(filePath);
            var headerLine = reader.ReadLine();
            if (headerLine is null)
            {
                yield break;
            }

            var headers = ParseCsvLine(headerLine);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var values = ParseCsvLine(line);
                var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i].Trim();
                    if (string.IsNullOrEmpty(header))
                    {
                        continue;
                    }

                    row[header] = i < values.Length ? values[i] : string.Empty;
                }

                yield return row;
            }
        }

        private static string[] ParseCsvLine(string line)
        {
            var values = new List<string>();
            var current = new System.Text.StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            current.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        values.Add(current.ToString());
                        current.Clear();
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }

            values.Add(current.ToString());
            return values.ToArray();
        }

        private static string ParseString(string? value) => value?.Trim() ?? string.Empty;

        private static int ParseInt(string? value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            throw new FormatException($"Invalid integer value: '{value}'");
        }

        private static int? ParseNullableInt(string? value)
        {
            value = value?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return ParseInt(value);
        }

        private static decimal ParseDecimal(string? value)
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            throw new FormatException($"Invalid decimal value: '{value}'");
        }

        private static DateTime ParseDateTime(string? value)
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var result))
            {
                return result;
            }

            throw new FormatException($"Invalid datetime value: '{value}'");
        }

        private static DateTime? ParseNullableDateTime(string? value)
        {
            value = value?.Trim();
            return string.IsNullOrEmpty(value) ? null : ParseDateTime(value);
        }

        private static LicenseType ParseLicenseType(string? value)
        {
            value = value?.Trim() ?? string.Empty;
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                return (LicenseType)intValue;
            }

            if (Enum.TryParse<LicenseType>(value, ignoreCase: true, out var licenseType))
            {
                return licenseType;
            }

            throw new FormatException($"Invalid LicenseType value: '{value}'");
        }
    }
}
