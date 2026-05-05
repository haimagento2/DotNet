using BlazorWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Company)
                .WithMany(co => co.Customers)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CustomerGroup)
                .WithMany(g => g.Customers)
                .HasForeignKey(c => c.CustomerGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            // CustomerGroups seed
            modelBuilder.Entity<CustomerGroup>().HasData(
                new CustomerGroup { Id = 1, Name = "VIP",        Description = "Premium clients with dedicated support",   CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 2, Name = "Enterprise", Description = "Large enterprise accounts",                CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 3, Name = "SMB",        Description = "Small and medium-sized businesses",        CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 4, Name = "Retail",     Description = "Individual retail customers",              CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 5, Name = "Partner",    Description = "Strategic business partners and resellers", CreatedAt = new DateTime(2024, 6, 1) }
            );

            // Companies seed
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1,  Name = "TechCorp",     Industry = "Technology",           City = "New York",     Phone = "+1-212-555-0101", Email = "contact@techcorp.com",     Website = "techcorp.com",     CreatedAt = new DateTime(2020, 3, 10) },
                new Company { Id = 2,  Name = "InnovateLabs", Industry = "Research & Dev",       City = "San Francisco",Phone = "+1-415-555-0102", Email = "hello@innovatelabs.com",   Website = "innovatelabs.com", CreatedAt = new DateTime(2019, 7, 22) },
                new Company { Id = 3,  Name = "DataSystems",  Industry = "Data Analytics",       City = "Los Angeles",  Phone = "+1-310-555-0103", Email = "info@datasystems.com",     Website = "datasystems.com",  CreatedAt = new DateTime(2018, 11, 5) },
                new Company { Id = 4,  Name = "CloudNext",    Industry = "Cloud Services",       City = "Chicago",      Phone = "+1-312-555-0104", Email = "sales@cloudnext.com",      Website = "cloudnext.com",    CreatedAt = new DateTime(2021, 1, 18) },
                new Company { Id = 5,  Name = "FastTrack",    Industry = "Logistics",            City = "Houston",      Phone = "+1-713-555-0105", Email = "ops@fasttrack.com",        Website = "fasttrack.com",    CreatedAt = new DateTime(2017, 9, 30) },
                new Company { Id = 6,  Name = "SecureNet",    Industry = "Cybersecurity",        City = "Phoenix",      Phone = "+1-602-555-0106", Email = "support@securenet.com",    Website = "securenet.com",    CreatedAt = new DateTime(2020, 5, 14) },
                new Company { Id = 7,  Name = "SmartBuild",   Industry = "Construction Tech",    City = "Philadelphia", Phone = "+1-215-555-0107", Email = "info@smartbuild.com",      Website = "smartbuild.com",   CreatedAt = new DateTime(2016, 4, 7)  },
                new Company { Id = 8,  Name = "VisionCore",   Industry = "Media Technology",     City = "San Antonio",  Phone = "+1-210-555-0108", Email = "hello@visioncore.com",     Website = "visioncore.com",   CreatedAt = new DateTime(2022, 2, 28) },
                new Company { Id = 9,  Name = "FutureAI",     Industry = "Artificial Intelligence",City = "San Diego",  Phone = "+1-619-555-0109", Email = "team@futureai.com",        Website = "futureai.com",     CreatedAt = new DateTime(2023, 8, 11) },
                new Company { Id = 10, Name = "PowerTech",    Industry = "Energy Technology",    City = "Dallas",       Phone = "+1-214-555-0110", Email = "contact@powertech.com",    Website = "powertech.com",    CreatedAt = new DateTime(2019, 12, 3) },
                new Company { Id = 11, Name = "QuantumEdge",  Industry = "Quantum Computing",    City = "Austin",       Phone = "+1-512-555-0111", Email = "info@quantumedge.com",     Website = "quantumedge.com",  CreatedAt = new DateTime(2023, 3, 20) },
                new Company { Id = 12, Name = "BrightPath",   Industry = "EdTech",               City = "Seattle",      Phone = "+1-206-555-0112", Email = "learn@brightpath.com",     Website = "brightpath.com",   CreatedAt = new DateTime(2021, 6, 9)  },
                new Company { Id = 13, Name = "NexaGroup",    Industry = "Consulting",           City = "Denver",       Phone = "+1-720-555-0113", Email = "hello@nexagroup.com",      Website = "nexagroup.com",    CreatedAt = new DateTime(2018, 8, 17) },
                new Company { Id = 14, Name = "AlphaLogic",   Industry = "Software",             City = "Boston",       Phone = "+1-617-555-0114", Email = "dev@alphalogic.com",       Website = "alphalogic.com",   CreatedAt = new DateTime(2020, 10, 6) },
                new Company { Id = 15, Name = "CoreWave",     Industry = "IoT",                  City = "Miami",        Phone = "+1-305-555-0115", Email = "connect@corewave.com",     Website = "corewave.com",     CreatedAt = new DateTime(2022, 4, 25) },
                new Company { Id = 16, Name = "PinnacleIO",   Industry = "Fintech",              City = "Atlanta",      Phone = "+1-404-555-0116", Email = "finance@pinnacleio.com",   Website = "pinnacleio.com",   CreatedAt = new DateTime(2021, 9, 13) },
                new Company { Id = 17, Name = "SkyLink",      Industry = "Telecommunications",   City = "Portland",     Phone = "+1-503-555-0117", Email = "network@skylink.com",      Website = "skylink.com",      CreatedAt = new DateTime(2017, 2, 19) },
                new Company { Id = 18, Name = "GridForce",    Industry = "Utilities",            City = "Minneapolis",  Phone = "+1-612-555-0118", Email = "grid@gridforce.com",        Website = "gridforce.com",    CreatedAt = new DateTime(2016, 11, 8) },
                new Company { Id = 19, Name = "NovaSpark",    Industry = "Clean Energy",         City = "Las Vegas",    Phone = "+1-702-555-0119", Email = "spark@novaspark.com",      Website = "novaspark.com",    CreatedAt = new DateTime(2024, 1, 30) },
                new Company { Id = 20, Name = "ZenithTech",   Industry = "Biotech",              City = "Nashville",    Phone = "+1-615-555-0120", Email = "bio@zenithtech.com",       Website = "zenithtech.com",   CreatedAt = new DateTime(2022, 7, 4)  }
            );

            // Customers seed  (CompanyId 1-20, GroupId cycles VIP→Enterprise→SMB→Retail→Partner)
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1,  FirstName = "John",        LastName = "Anderson", Email = "john.anderson@example.com",        Phone = "+1-555-0101", City = "New York",     CompanyId = 1,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 1, 15) },
                new Customer { Id = 2,  FirstName = "Sarah",       LastName = "Bennett",  Email = "sarah.bennett@example.com",        Phone = "+1-555-0102", City = "San Francisco",CompanyId = 2,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 2, 3)  },
                new Customer { Id = 3,  FirstName = "Michael",     LastName = "Chen",     Email = "michael.chen@example.com",         Phone = "+1-555-0103", City = "Los Angeles",  CompanyId = 3,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 2, 20) },
                new Customer { Id = 4,  FirstName = "Emily",       LastName = "Davis",    Email = "emily.davis@example.com",          Phone = "+1-555-0104", City = "Chicago",      CompanyId = 4,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 3, 8)  },
                new Customer { Id = 5,  FirstName = "David",       LastName = "Edwards",  Email = "david.edwards@example.com",        Phone = "+1-555-0105", City = "Houston",      CompanyId = 5,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 3, 25) },
                new Customer { Id = 6,  FirstName = "Jessica",     LastName = "Garcia",   Email = "jessica.garcia@example.com",       Phone = "+1-555-0106", City = "Phoenix",      CompanyId = 6,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 4, 11) },
                new Customer { Id = 7,  FirstName = "Robert",      LastName = "Harris",   Email = "robert.harris@example.com",        Phone = "+1-555-0107", City = "Philadelphia", CompanyId = 7,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 5, 2)  },
                new Customer { Id = 8,  FirstName = "Amanda",      LastName = "Johnson",  Email = "amanda.johnson@example.com",       Phone = "+1-555-0108", City = "San Antonio",  CompanyId = 8,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 5, 19) },
                new Customer { Id = 9,  FirstName = "Christopher", LastName = "King",     Email = "christopher.king@example.com",     Phone = "+1-555-0109", City = "San Diego",    CompanyId = 9,  CustomerGroupId = 3, CreatedAt = new DateTime(2025, 6, 7)  },
                new Customer { Id = 10, FirstName = "Victoria",    LastName = "Lee",      Email = "victoria.lee@example.com",         Phone = "+1-555-0110", City = "Dallas",       CompanyId = 10, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 7, 14) },
                new Customer { Id = 11, FirstName = "Nathan",      LastName = "Martinez", Email = "nathan.martinez@example.com",      Phone = "+1-555-0111", City = "Austin",       CompanyId = 11, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 7, 30) },
                new Customer { Id = 12, FirstName = "Olivia",      LastName = "Nelson",   Email = "olivia.nelson@example.com",        Phone = "+1-555-0112", City = "Seattle",      CompanyId = 12, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 8, 16) },
                new Customer { Id = 13, FirstName = "Ethan",       LastName = "Parker",   Email = "ethan.parker@example.com",         Phone = "+1-555-0113", City = "Denver",       CompanyId = 13, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 9, 4)  },
                new Customer { Id = 14, FirstName = "Sophia",      LastName = "Quinn",    Email = "sophia.quinn@example.com",         Phone = "+1-555-0114", City = "Boston",       CompanyId = 14, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 9, 22) },
                new Customer { Id = 15, FirstName = "Liam",        LastName = "Rivera",   Email = "liam.rivera@example.com",          Phone = "+1-555-0115", City = "Miami",        CompanyId = 15, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 10, 10) },
                new Customer { Id = 16, FirstName = "Isabella",    LastName = "Scott",    Email = "isabella.scott@example.com",       Phone = "+1-555-0116", City = "Atlanta",      CompanyId = 16, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 10, 28) },
                new Customer { Id = 17, FirstName = "Mason",       LastName = "Turner",   Email = "mason.turner@example.com",         Phone = "+1-555-0117", City = "Portland",     CompanyId = 17, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 11, 15) },
                new Customer { Id = 18, FirstName = "Ava",         LastName = "Underwood",Email = "ava.underwood@example.com",        Phone = "+1-555-0118", City = "Minneapolis",  CompanyId = 18, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 12, 3)  },
                new Customer { Id = 19, FirstName = "Logan",       LastName = "Vasquez",  Email = "logan.vasquez@example.com",        Phone = "+1-555-0119", City = "Las Vegas",    CompanyId = 19, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 12, 21) },
                new Customer { Id = 20, FirstName = "Mia",         LastName = "Walsh",    Email = "mia.walsh@example.com",            Phone = "+1-555-0120", City = "Nashville",    CompanyId = 20, CustomerGroupId = 5, CreatedAt = new DateTime(2026, 1, 9)  }
            );
        }
    }
}
