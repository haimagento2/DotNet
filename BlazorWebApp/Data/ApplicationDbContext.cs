using BlazorWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer>      Customers      { get; set; }
        public DbSet<Admin>         Admins         { get; set; }
        public DbSet<Company>       Companies      { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }
        public DbSet<AppProgram>    Programs       { get; set; }
        public DbSet<License>       Licenses       { get; set; }
        public DbSet<LicenseMember> LicenseMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppProgram>().ToTable("Programs");

            // License primary key
            modelBuilder.Entity<License>().HasKey(l => l.Id);
            modelBuilder.Entity<License>().HasIndex(l => l.LicenseKey).IsUnique();

            // Customer FK relationships
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Company).WithMany(co => co.Customers)
                .HasForeignKey(c => c.CompanyId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CustomerGroup).WithMany(g => g.Customers)
                .HasForeignKey(c => c.CustomerGroupId).OnDelete(DeleteBehavior.SetNull);

            // License FK relationships
            modelBuilder.Entity<License>()
                .HasOne(l => l.Program).WithMany(p => p.Licenses)
                .HasForeignKey(l => l.ProgramId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<License>()
                .HasOne(l => l.Company).WithMany()
                .HasForeignKey(l => l.CompanyId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<License>()
                .HasOne(l => l.CustomerGroup).WithMany()
                .HasForeignKey(l => l.CustomerGroupId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<License>()
                .HasOne(l => l.Owner).WithMany()
                .HasForeignKey(l => l.OwnerId).OnDelete(DeleteBehavior.SetNull);

            // LicenseMember FK relationships
            modelBuilder.Entity<LicenseMember>()
                .HasOne(lm => lm.License).WithMany(l => l.Members)
                .HasForeignKey(lm => lm.LicenseId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LicenseMember>()
                .HasOne(lm => lm.Customer).WithMany()
                .HasForeignKey(lm => lm.CustomerId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LicenseMember>()
                .HasOne(lm => lm.Owner).WithMany()
                .HasForeignKey(lm => lm.OwnerId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LicenseMember>()
                .HasIndex(lm => new { lm.LicenseId, lm.CustomerId }).IsUnique();

            // ── CustomerGroup seed ────────────────────────────────────────────────
            modelBuilder.Entity<CustomerGroup>().HasData(
                new CustomerGroup { Id = 1, Name = "VIP",        Description = "Premium clients with dedicated support",    CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 2, Name = "Enterprise", Description = "Large enterprise accounts",                 CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 3, Name = "SMB",        Description = "Small and medium-sized businesses",         CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 4, Name = "Retail",     Description = "Individual retail customers",               CreatedAt = new DateTime(2024, 6, 1) },
                new CustomerGroup { Id = 5, Name = "Partner",    Description = "Strategic business partners and resellers", CreatedAt = new DateTime(2024, 6, 1) }
            );

            // ── Company seed ──────────────────────────────────────────────────────
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1,  Name = "TechCorp",     Industry = "Technology",             City = "New York",     Phone = "+1-212-555-0101", Email = "contact@techcorp.com",   Website = "techcorp.com",     CreatedAt = new DateTime(2020, 3, 10) },
                new Company { Id = 2,  Name = "InnovateLabs", Industry = "Research & Dev",         City = "San Francisco",Phone = "+1-415-555-0102", Email = "hello@innovatelabs.com", Website = "innovatelabs.com", CreatedAt = new DateTime(2019, 7, 22) },
                new Company { Id = 3,  Name = "DataSystems",  Industry = "Data Analytics",         City = "Los Angeles",  Phone = "+1-310-555-0103", Email = "info@datasystems.com",   Website = "datasystems.com",  CreatedAt = new DateTime(2018, 11, 5) },
                new Company { Id = 4,  Name = "CloudNext",    Industry = "Cloud Services",         City = "Chicago",      Phone = "+1-312-555-0104", Email = "sales@cloudnext.com",    Website = "cloudnext.com",    CreatedAt = new DateTime(2021, 1, 18) },
                new Company { Id = 5,  Name = "FastTrack",    Industry = "Logistics",              City = "Houston",      Phone = "+1-713-555-0105", Email = "ops@fasttrack.com",      Website = "fasttrack.com",    CreatedAt = new DateTime(2017, 9, 30) },
                new Company { Id = 6,  Name = "SecureNet",    Industry = "Cybersecurity",          City = "Phoenix",      Phone = "+1-602-555-0106", Email = "support@securenet.com",  Website = "securenet.com",    CreatedAt = new DateTime(2020, 5, 14) },
                new Company { Id = 7,  Name = "SmartBuild",   Industry = "Construction Tech",      City = "Philadelphia", Phone = "+1-215-555-0107", Email = "info@smartbuild.com",    Website = "smartbuild.com",   CreatedAt = new DateTime(2016, 4, 7)  },
                new Company { Id = 8,  Name = "VisionCore",   Industry = "Media Technology",       City = "San Antonio",  Phone = "+1-210-555-0108", Email = "hello@visioncore.com",   Website = "visioncore.com",   CreatedAt = new DateTime(2022, 2, 28) },
                new Company { Id = 9,  Name = "FutureAI",     Industry = "Artificial Intelligence",City = "San Diego",    Phone = "+1-619-555-0109", Email = "team@futureai.com",      Website = "futureai.com",     CreatedAt = new DateTime(2023, 8, 11) },
                new Company { Id = 10, Name = "PowerTech",    Industry = "Energy Technology",      City = "Dallas",       Phone = "+1-214-555-0110", Email = "contact@powertech.com",  Website = "powertech.com",    CreatedAt = new DateTime(2019, 12, 3) },
                new Company { Id = 11, Name = "QuantumEdge",  Industry = "Quantum Computing",      City = "Austin",       Phone = "+1-512-555-0111", Email = "info@quantumedge.com",   Website = "quantumedge.com",  CreatedAt = new DateTime(2023, 3, 20) },
                new Company { Id = 12, Name = "BrightPath",   Industry = "EdTech",                 City = "Seattle",      Phone = "+1-206-555-0112", Email = "learn@brightpath.com",   Website = "brightpath.com",   CreatedAt = new DateTime(2021, 6, 9)  },
                new Company { Id = 13, Name = "NexaGroup",    Industry = "Consulting",             City = "Denver",       Phone = "+1-720-555-0113", Email = "hello@nexagroup.com",    Website = "nexagroup.com",    CreatedAt = new DateTime(2018, 8, 17) },
                new Company { Id = 14, Name = "AlphaLogic",   Industry = "Software",               City = "Boston",       Phone = "+1-617-555-0114", Email = "dev@alphalogic.com",     Website = "alphalogic.com",   CreatedAt = new DateTime(2020, 10, 6) },
                new Company { Id = 15, Name = "CoreWave",     Industry = "IoT",                    City = "Miami",        Phone = "+1-305-555-0115", Email = "connect@corewave.com",   Website = "corewave.com",     CreatedAt = new DateTime(2022, 4, 25) },
                new Company { Id = 16, Name = "PinnacleIO",   Industry = "Fintech",                City = "Atlanta",      Phone = "+1-404-555-0116", Email = "finance@pinnacleio.com", Website = "pinnacleio.com",   CreatedAt = new DateTime(2021, 9, 13) },
                new Company { Id = 17, Name = "SkyLink",      Industry = "Telecommunications",     City = "Portland",     Phone = "+1-503-555-0117", Email = "network@skylink.com",    Website = "skylink.com",      CreatedAt = new DateTime(2017, 2, 19) },
                new Company { Id = 18, Name = "GridForce",    Industry = "Utilities",              City = "Minneapolis",  Phone = "+1-612-555-0118", Email = "grid@gridforce.com",     Website = "gridforce.com",    CreatedAt = new DateTime(2016, 11, 8) },
                new Company { Id = 19, Name = "NovaSpark",    Industry = "Clean Energy",           City = "Las Vegas",    Phone = "+1-702-555-0119", Email = "spark@novaspark.com",    Website = "novaspark.com",    CreatedAt = new DateTime(2024, 1, 30) },
                new Company { Id = 20, Name = "ZenithTech",   Industry = "Biotech",                City = "Nashville",    Phone = "+1-615-555-0120", Email = "bio@zenithtech.com",     Website = "zenithtech.com",   CreatedAt = new DateTime(2022, 7, 4)  }
            );

            // ── Customer seed ─────────────────────────────────────────────────────
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1,  FirstName = "John",        LastName = "Anderson", Email = "john.anderson@example.com",    Phone = "+1-555-0101", City = "New York",     CompanyId = 1,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 1, 15)  },
                new Customer { Id = 2,  FirstName = "Sarah",       LastName = "Bennett",  Email = "sarah.bennett@example.com",    Phone = "+1-555-0102", City = "San Francisco",CompanyId = 2,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 2, 3)   },
                new Customer { Id = 3,  FirstName = "Michael",     LastName = "Chen",     Email = "michael.chen@example.com",     Phone = "+1-555-0103", City = "Los Angeles",  CompanyId = 3,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 2, 20)  },
                new Customer { Id = 4,  FirstName = "Emily",       LastName = "Davis",    Email = "emily.davis@example.com",      Phone = "+1-555-0104", City = "Chicago",      CompanyId = 4,  CustomerGroupId = 1, CreatedAt = new DateTime(2025, 3, 8)   },
                new Customer { Id = 5,  FirstName = "David",       LastName = "Edwards",  Email = "david.edwards@example.com",    Phone = "+1-555-0105", City = "Houston",      CompanyId = 5,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 3, 25)  },
                new Customer { Id = 6,  FirstName = "Jessica",     LastName = "Garcia",   Email = "jessica.garcia@example.com",   Phone = "+1-555-0106", City = "Phoenix",      CompanyId = 6,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 4, 11)  },
                new Customer { Id = 7,  FirstName = "Robert",      LastName = "Harris",   Email = "robert.harris@example.com",    Phone = "+1-555-0107", City = "Philadelphia", CompanyId = 7,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 5, 2)   },
                new Customer { Id = 8,  FirstName = "Amanda",      LastName = "Johnson",  Email = "amanda.johnson@example.com",   Phone = "+1-555-0108", City = "San Antonio",  CompanyId = 8,  CustomerGroupId = 2, CreatedAt = new DateTime(2025, 5, 19)  },
                new Customer { Id = 9,  FirstName = "Christopher", LastName = "King",     Email = "christopher.king@example.com", Phone = "+1-555-0109", City = "San Diego",    CompanyId = 9,  CustomerGroupId = 3, CreatedAt = new DateTime(2025, 6, 7)   },
                new Customer { Id = 10, FirstName = "Victoria",    LastName = "Lee",      Email = "victoria.lee@example.com",     Phone = "+1-555-0110", City = "Dallas",       CompanyId = 10, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 7, 14)  },
                new Customer { Id = 11, FirstName = "Nathan",      LastName = "Martinez", Email = "nathan.martinez@example.com",  Phone = "+1-555-0111", City = "Austin",       CompanyId = 11, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 7, 30)  },
                new Customer { Id = 12, FirstName = "Olivia",      LastName = "Nelson",   Email = "olivia.nelson@example.com",    Phone = "+1-555-0112", City = "Seattle",      CompanyId = 12, CustomerGroupId = 3, CreatedAt = new DateTime(2025, 8, 16)  },
                new Customer { Id = 13, FirstName = "Ethan",       LastName = "Parker",   Email = "ethan.parker@example.com",     Phone = "+1-555-0113", City = "Denver",       CompanyId = 13, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 9, 4)   },
                new Customer { Id = 14, FirstName = "Sophia",      LastName = "Quinn",    Email = "sophia.quinn@example.com",     Phone = "+1-555-0114", City = "Boston",       CompanyId = 14, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 9, 22)  },
                new Customer { Id = 15, FirstName = "Liam",        LastName = "Rivera",   Email = "liam.rivera@example.com",      Phone = "+1-555-0115", City = "Miami",        CompanyId = 15, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 10, 10) },
                new Customer { Id = 16, FirstName = "Isabella",    LastName = "Scott",    Email = "isabella.scott@example.com",   Phone = "+1-555-0116", City = "Atlanta",      CompanyId = 16, CustomerGroupId = 4, CreatedAt = new DateTime(2025, 10, 28) },
                new Customer { Id = 17, FirstName = "Mason",       LastName = "Turner",   Email = "mason.turner@example.com",     Phone = "+1-555-0117", City = "Portland",     CompanyId = 17, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 11, 15) },
                new Customer { Id = 18, FirstName = "Ava",         LastName = "Underwood",Email = "ava.underwood@example.com",    Phone = "+1-555-0118", City = "Minneapolis",  CompanyId = 18, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 12, 3)  },
                new Customer { Id = 19, FirstName = "Logan",       LastName = "Vasquez",  Email = "logan.vasquez@example.com",    Phone = "+1-555-0119", City = "Las Vegas",    CompanyId = 19, CustomerGroupId = 5, CreatedAt = new DateTime(2025, 12, 21) },
                new Customer { Id = 20, FirstName = "Mia",         LastName = "Walsh",    Email = "mia.walsh@example.com",        Phone = "+1-555-0120", City = "Nashville",    CompanyId = 20, CustomerGroupId = 5, CreatedAt = new DateTime(2026, 1, 9)   }
            );

            // ── Program seed ──────────────────────────────────────────────────────
            modelBuilder.Entity<AppProgram>().HasData(
                new AppProgram { Id = 1, Name = "BasicCRM",      Description = "Essential CRM tools for small teams",            Version = "1.0", Price = 99.99m,   CreatedAt = new DateTime(2023, 1, 1) },
                new AppProgram { Id = 2, Name = "ProCRM",        Description = "Advanced CRM with analytics and automation",     Version = "2.0", Price = 299.99m,  CreatedAt = new DateTime(2023, 3, 1) },
                new AppProgram { Id = 3, Name = "EnterpriseCRM", Description = "Full-scale enterprise CRM suite",                Version = "3.0", Price = 999.99m,  CreatedAt = new DateTime(2023, 6, 1) },
                new AppProgram { Id = 4, Name = "DataAnalytics", Description = "Business intelligence and reporting module",     Version = "1.5", Price = 199.99m,  CreatedAt = new DateTime(2023, 9, 1) },
                new AppProgram { Id = 5, Name = "SecurityPro",   Description = "Security compliance and audit management",       Version = "2.1", Price = 149.99m,  CreatedAt = new DateTime(2024, 1, 1) }
            );

            // ── License seed ──────────────────────────────────────────────────────
            modelBuilder.Entity<License>().HasData(
                new License { Id = 1,  LicenseKey = 1001, ProgramId = 1,  CompanyId = 1,  CustomerGroupId = null, OwnerId = 1,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 1, 1),  ExpiryDate = new DateTime(2026, 1, 1),  MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025, 1, 1) },
                new License { Id = 2,  LicenseKey = 1002, ProgramId = 2,  CompanyId = 2,  CustomerGroupId = null, OwnerId = 2,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 2, 1),  ExpiryDate = new DateTime(2026, 2, 1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025, 2, 1) },
                new License { Id = 3,  LicenseKey = 1003, ProgramId = 3,  CompanyId = 3,  CustomerGroupId = null, OwnerId = 3,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 3, 1),  ExpiryDate = new DateTime(2026, 3, 1),  MaxMembers = 50, Status = "Active",    CreatedAt = new DateTime(2025, 3, 1) },
                new License { Id = 4,  LicenseKey = 1004, ProgramId = 4,  CompanyId = 4,  CustomerGroupId = null, OwnerId = 4,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 4, 1),  ExpiryDate = new DateTime(2026, 4, 1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025, 4, 1) },
                new License { Id = 5,  LicenseKey = 1005, ProgramId = 5,  CompanyId = 5,  CustomerGroupId = null, OwnerId = 5,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 5, 1),  ExpiryDate = new DateTime(2026, 5, 1),  MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025, 5, 1) },
                new License { Id = 6,  LicenseKey = 1006, ProgramId = 1,  CompanyId = null, CustomerGroupId = 1,  OwnerId = 1,  SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025, 6, 1),  ExpiryDate = new DateTime(2026, 6, 1),  MaxMembers = 15, Status = "Active",    CreatedAt = new DateTime(2025, 6, 1) },
                new License { Id = 7,  LicenseKey = 1007, ProgramId = 2,  CompanyId = null, CustomerGroupId = 2,  OwnerId = 2,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 7, 1),  ExpiryDate = new DateTime(2026, 7, 1),  MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025, 7, 1) },
                new License { Id = 8,  LicenseKey = 1008, ProgramId = 3,  CompanyId = 8,  CustomerGroupId = null, OwnerId = 8,  SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025, 1, 15), ExpiryDate = new DateTime(2026, 1, 15), MaxMembers = 25, Status = "Suspended", CreatedAt = new DateTime(2025, 1, 15) },
                new License { Id = 9,  LicenseKey = 1009, ProgramId = 4,  CompanyId = null, CustomerGroupId = 3,  OwnerId = 9,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 2, 15), ExpiryDate = new DateTime(2026, 2, 15), MaxMembers = 30, Status = "Active",    CreatedAt = new DateTime(2025, 2, 15) },
                new License { Id = 10, LicenseKey = 1010, ProgramId = 5,  CompanyId = 10, CustomerGroupId = null, OwnerId = 10, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 3, 15), ExpiryDate = new DateTime(2026, 3, 15), MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025, 3, 15) },
                new License { Id = 11, LicenseKey = 1011, ProgramId = 1,  CompanyId = null, CustomerGroupId = 4,  OwnerId = 11, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 4, 15), ExpiryDate = new DateTime(2025, 10, 15), MaxMembers = 15, Status = "Expired",   CreatedAt = new DateTime(2025, 4, 15) },
                new License { Id = 12, LicenseKey = 1012, ProgramId = 2,  CompanyId = 12, CustomerGroupId = null, OwnerId = 12, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 5, 15), ExpiryDate = new DateTime(2026, 5, 15), MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025, 5, 15) },
                new License { Id = 13, LicenseKey = 1013, ProgramId = 3,  CompanyId = null, CustomerGroupId = 5,  OwnerId = 13, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025, 6, 15), ExpiryDate = new DateTime(2026, 6, 15), MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025, 6, 15) },
                new License { Id = 14, LicenseKey = 1014, ProgramId = 4,  CompanyId = 14, CustomerGroupId = null, OwnerId = 14, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 7, 15), ExpiryDate = new DateTime(2026, 7, 15), MaxMembers = 10, Status = "Suspended", CreatedAt = new DateTime(2025, 7, 15) },
                new License { Id = 15, LicenseKey = 1015, ProgramId = 5,  CompanyId = 15, CustomerGroupId = null, OwnerId = 15, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 8, 15), ExpiryDate = new DateTime(2026, 8, 15), MaxMembers = 5,  Status = "Active",    CreatedAt = new DateTime(2025, 8, 15) },
                new License { Id = 16, LicenseKey = 1016, ProgramId = 1,  CompanyId = null, CustomerGroupId = 1,  OwnerId = 16, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 9, 15), ExpiryDate = new DateTime(2026, 9, 15), MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2025, 9, 15) },
                new License { Id = 17, LicenseKey = 1017, ProgramId = 2,  CompanyId = 17, CustomerGroupId = null, OwnerId = 17, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2025, 10, 15), ExpiryDate = new DateTime(2026, 10, 15), MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2025, 10, 15) },
                new License { Id = 18, LicenseKey = 1018, ProgramId = 3,  CompanyId = null, CustomerGroupId = 2,  OwnerId = 18, SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2025, 11, 15), ExpiryDate = new DateTime(2026, 11, 15), MaxMembers = 15, Status = "Active",    CreatedAt = new DateTime(2025, 11, 15) },
                new License { Id = 19, LicenseKey = 1019, ProgramId = 4,  CompanyId = 19, CustomerGroupId = null, OwnerId = 19, SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2025, 12, 15), ExpiryDate = new DateTime(2026, 12, 15), MaxMembers = 25, Status = "Active",    CreatedAt = new DateTime(2025, 12, 15) },
                new License { Id = 20, LicenseKey = 1020, ProgramId = 5,  CompanyId = null, CustomerGroupId = 3,  OwnerId = 20, SubLicenseType = LicenseType.Seminar,    StartDate = new DateTime(2026, 1, 15),  ExpiryDate = new DateTime(2027, 1, 15),  MaxMembers = 5,  Status = "Expired",   CreatedAt = new DateTime(2026, 1, 15) },
                new License { Id = 21, LicenseKey = 1021, ProgramId = 1,  CompanyId = 2,  CustomerGroupId = null, OwnerId = 1,  SubLicenseType = LicenseType.TeamSize,   StartDate = new DateTime(2026, 2, 1),  ExpiryDate = new DateTime(2027, 2, 1),  MaxMembers = 10, Status = "Active",    CreatedAt = new DateTime(2026, 2, 1) },
                new License { Id = 22, LicenseKey = 1022, ProgramId = 2,  CompanyId = 4,  CustomerGroupId = null, OwnerId = 2,  SubLicenseType = LicenseType.QtyPurchase, StartDate = new DateTime(2026, 3, 1),  ExpiryDate = new DateTime(2027, 3, 1),  MaxMembers = 20, Status = "Active",    CreatedAt = new DateTime(2026, 3, 1) }
            );

            // ── LicenseMember seed ────────────────────────────────────────────────
            modelBuilder.Entity<LicenseMember>().HasData(
                new LicenseMember { Id = 1,  LicenseId = 1,  CustomerId = 2,  OwnerId = 1,  AssignedAt = new DateTime(2025, 1, 10), CreatedAt = new DateTime(2025, 1, 10) },
                new LicenseMember { Id = 2,  LicenseId = 1,  CustomerId = 3,  OwnerId = 1,  AssignedAt = new DateTime(2025, 1, 12), CreatedAt = new DateTime(2025, 1, 12) },
                new LicenseMember { Id = 3,  LicenseId = 2,  CustomerId = 4,  OwnerId = 2,  AssignedAt = new DateTime(2025, 2, 5),  CreatedAt = new DateTime(2025, 2, 5) },
                new LicenseMember { Id = 4,  LicenseId = 2,  CustomerId = 5,  OwnerId = 2,  AssignedAt = new DateTime(2025, 2, 8),  CreatedAt = new DateTime(2025, 2, 8) },
                new LicenseMember { Id = 5,  LicenseId = 3,  CustomerId = 6,  OwnerId = 3,  AssignedAt = new DateTime(2025, 3, 5),  CreatedAt = new DateTime(2025, 3, 5) },
                new LicenseMember { Id = 6,  LicenseId = 3,  CustomerId = 7,  OwnerId = 3,  AssignedAt = new DateTime(2025, 3, 6),  CreatedAt = new DateTime(2025, 3, 6) },
                new LicenseMember { Id = 7,  LicenseId = 3,  CustomerId = 8,  OwnerId = 3,  AssignedAt = new DateTime(2025, 3, 7),  CreatedAt = new DateTime(2025, 3, 7) },
                new LicenseMember { Id = 8,  LicenseId = 4,  CustomerId = 9,  OwnerId = 4,  AssignedAt = new DateTime(2025, 4, 3),  CreatedAt = new DateTime(2025, 4, 3) },
                new LicenseMember { Id = 9,  LicenseId = 5,  CustomerId = 10, OwnerId = 5,  AssignedAt = new DateTime(2025, 5, 2),  CreatedAt = new DateTime(2025, 5, 2) },
                new LicenseMember { Id = 10, LicenseId = 5,  CustomerId = 11, OwnerId = 5,  AssignedAt = new DateTime(2025, 5, 4),  CreatedAt = new DateTime(2025, 5, 4) },
                new LicenseMember { Id = 11, LicenseId = 6,  CustomerId = 12, OwnerId = 1,  AssignedAt = new DateTime(2025, 6, 2),  CreatedAt = new DateTime(2025, 6, 2) },
                new LicenseMember { Id = 12, LicenseId = 7,  CustomerId = 13, OwnerId = 2,  AssignedAt = new DateTime(2025, 6, 5),  CreatedAt = new DateTime(2025, 6, 5) },
                new LicenseMember { Id = 13, LicenseId = 7,  CustomerId = 14, OwnerId = 2,  AssignedAt = new DateTime(2025, 6, 7),  CreatedAt = new DateTime(2025, 6, 7) },
                new LicenseMember { Id = 14, LicenseId = 8,  CustomerId = 15, OwnerId = 8,  AssignedAt = new DateTime(2025, 1, 20), CreatedAt = new DateTime(2025, 1, 20) },
                new LicenseMember { Id = 15, LicenseId = 8,  CustomerId = 16, OwnerId = 8,  AssignedAt = new DateTime(2025, 1, 22), CreatedAt = new DateTime(2025, 1, 22) },
                new LicenseMember { Id = 16, LicenseId = 9,  CustomerId = 17, OwnerId = 9,  AssignedAt = new DateTime(2025, 2, 20), CreatedAt = new DateTime(2025, 2, 20) },
                new LicenseMember { Id = 17, LicenseId = 10, CustomerId = 18, OwnerId = 10, AssignedAt = new DateTime(2025, 3, 20), CreatedAt = new DateTime(2025, 3, 20) },
                new LicenseMember { Id = 18, LicenseId = 10, CustomerId = 19, OwnerId = 10, AssignedAt = new DateTime(2025, 3, 22), CreatedAt = new DateTime(2025, 3, 22) },
                new LicenseMember { Id = 19, LicenseId = 11, CustomerId = 20, OwnerId = 11, AssignedAt = new DateTime(2025, 4, 20), CreatedAt = new DateTime(2025, 4, 20) },
                new LicenseMember { Id = 20, LicenseId = 12, CustomerId = 1,  OwnerId = 12, AssignedAt = new DateTime(2025, 5, 20), CreatedAt = new DateTime(2025, 5, 20) },
                new LicenseMember { Id = 21, LicenseId = 13, CustomerId = 2,  OwnerId = 13, AssignedAt = new DateTime(2025, 6, 20), CreatedAt = new DateTime(2025, 6, 20) },
                new LicenseMember { Id = 22, LicenseId = 14, CustomerId = 3,  OwnerId = 14, AssignedAt = new DateTime(2025, 7, 20), CreatedAt = new DateTime(2025, 7, 20) },
                new LicenseMember { Id = 23, LicenseId = 15, CustomerId = 4,  OwnerId = 15, AssignedAt = new DateTime(2025, 8, 20), CreatedAt = new DateTime(2025, 8, 20) },
                new LicenseMember { Id = 24, LicenseId = 16, CustomerId = 5,  OwnerId = 16, AssignedAt = new DateTime(2025, 9, 20), CreatedAt = new DateTime(2025, 9, 20) },
                new LicenseMember { Id = 25, LicenseId = 17, CustomerId = 6,  OwnerId = 17, AssignedAt = new DateTime(2025, 10, 20),CreatedAt = new DateTime(2025, 10, 20) },
                new LicenseMember { Id = 26, LicenseId = 18, CustomerId = 7,  OwnerId = 18, AssignedAt = new DateTime(2025, 11, 20),CreatedAt = new DateTime(2025, 11, 20) },
                new LicenseMember { Id = 27, LicenseId = 19, CustomerId = 8,  OwnerId = 19, AssignedAt = new DateTime(2025, 12, 20),CreatedAt = new DateTime(2025, 12, 20) },
                new LicenseMember { Id = 28, LicenseId = 20, CustomerId = 9,  OwnerId = 20, AssignedAt = new DateTime(2026, 1, 20), CreatedAt = new DateTime(2026, 1, 20) },
                new LicenseMember { Id = 29, LicenseId = 21, CustomerId = 10, OwnerId = 1,  AssignedAt = new DateTime(2026, 2, 10), CreatedAt = new DateTime(2026, 2, 10) },
                new LicenseMember { Id = 30, LicenseId = 22, CustomerId = 11, OwnerId = 2,  AssignedAt = new DateTime(2026, 3, 10), CreatedAt = new DateTime(2026, 3, 10) }
            );
        }
    }
}
