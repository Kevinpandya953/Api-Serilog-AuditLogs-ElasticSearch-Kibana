using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class BikeStoresDbContext : DbContext
    {
        public BikeStoresDbContext(DbContextOptions<BikeStoresDbContext> options) : base(options)
        {
            
        }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<AuditLogs> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brands>().ToTable("brands", schema: "production");
            modelBuilder.Entity<Categories>().ToTable("categories", schema: "production");
            modelBuilder.Entity<Products>().ToTable("products", schema: "production");
            modelBuilder.Entity<Customers>().ToTable("customers", schema: "sales");
            modelBuilder.Entity<AuditLogs>();


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>()
            .HasOne(p => p.Brands)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.brand_id)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Categories)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.category_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
