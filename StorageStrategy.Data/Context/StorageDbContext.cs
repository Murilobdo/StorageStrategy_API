using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Mappings;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Context
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {

        }

        public StorageDbContext()
        {

        }

        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Company> Company { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {   
            base.OnConfiguring(options);
            options.EnableSensitiveDataLogging();
            options.UseSqlServer("Server=MURILOBDO\\LOCALHOST;Initial Catalog=StorageStrategy; Integrated Security = true; TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasData(new Company(companyId: 1, name: "Bar do Murps", description: "Bar", createAt: DateTime.Now),
                        new Company(companyId: 2, name: "Rei do Baco", description: "Tabacaria", createAt: DateTime.Now));

            modelBuilder.ApplyConfiguration(new CompanyMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
