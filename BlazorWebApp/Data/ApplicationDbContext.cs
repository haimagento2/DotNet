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
        public DbSet<Product>       Products       { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<CategoryProductLink> CategoryProductLinks { get; set; }
        public DbSet<ProgramProductAssociated> ProgramProductAssociations { get; set; }
        public DbSet<License>       Licenses       { get; set; }
        public DbSet<LicenseMember> LicenseMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppProgram>().ToTable("Programs");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategories");
            modelBuilder.Entity<CategoryProductLink>().ToTable("CategoryProductLinks");
            modelBuilder.Entity<ProgramProductAssociated>().ToTable("ProgramProductAssociated");

            // Product columns
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("product_id");
                entity.Property(p => p.Name).HasColumnName("name").IsRequired();
                entity.Property(p => p.SKU).HasColumnName("sku").IsRequired();
                entity.Property(p => p.Type).HasColumnName("type").IsRequired();
                entity.Property(p => p.Period).HasColumnName("Period").IsRequired();
                entity.Property(p => p.EnableSubscriptionLinking).HasColumnName("EnableSubscriptionLinking");
                entity.Property(p => p.LicenseOnly).HasColumnName("LicenseOnly");
                entity.Property(p => p.CommunityOnly).HasColumnName("CommunityOnly");
                entity.Property(p => p.SendEmailInstruction).HasColumnName("SendEmailInstruction");
                entity.Property(p => p.ProductCategoryId).HasColumnName("ProductCategoryId");
                entity.Property(p => p.Price).HasColumnName("price");
                entity.Property(p => p.Image).HasColumnName("image").IsRequired();
                entity.Property(p => p.Description).HasColumnName("description").IsRequired();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.Property(c => c.Id).HasColumnName("category_id");
                entity.Property(c => c.Name).HasColumnName("name").IsRequired();
                entity.Property(c => c.ParentId).HasColumnName("parent_id");
                entity.Property(c => c.Image).HasColumnName("image").IsRequired();
                entity.Property(c => c.Description).HasColumnName("description").IsRequired();
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.ChildCategories)
                    .HasForeignKey(c => c.ParentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CategoryProductLink>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.CategoryProductLinks)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.CategoryProductLinks)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

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

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProgramProductAssociated>(entity =>
            {
                entity.HasKey(e => new { e.ProgramId, e.ProductId });

                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Period).HasColumnName("period");
                entity.Property(e => e.CustomPeriodDays).HasColumnName("custom_period");
                entity.Property(e => e.LinkingLicense).HasColumnName("linking_license");
                entity.Property(e => e.CommunityOnly).HasColumnName("community_only");

                entity.HasOne(e => e.Program)
                    .WithMany(p => p.ProgramProductAssociations)
                    .HasForeignKey(e => e.ProgramId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProgramProductAssociations)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
