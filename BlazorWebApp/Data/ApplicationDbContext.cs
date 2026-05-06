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
        }
    }
}
