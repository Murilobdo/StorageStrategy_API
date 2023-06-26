using Isopoh.Cryptography.Argon2;
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
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<CompanyEntity> Company { get; set; }
        public DbSet<EmployeeEntity> Employee { get; set; }
        public DbSet<CommandEntity> Command { get; set; }
        public DbSet<CommandItemEntity> CommandItems { get; set; }
        public DbSet<ExpensesEntity> Expenses { get; set; }
        public DbSet<ExpensesTypeEntity> ExpensesType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {   
            base.OnConfiguring(options);
            options.UseSqlServer("Server=tcp:elitegate.database.windows.net,1433;Initial Catalog=Storage Strategy;Persist Security Info=False;User ID=murilobdo;Password=Fib@1123581321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CompanyEntity adminCompany = new(companyId: 1, name: "Admin Company", description: "Admin", 
                createAt: DateTime.Now, validate: DateTime.Now.AddYears(10));

            modelBuilder.Entity<CompanyEntity>().HasData(adminCompany);

             modelBuilder.Entity<EmployeeEntity>().HasData(new EmployeeEntity{
                EmployeeId = 1,
                Name = "Murilo Bernardes (Admin)",
                Email = "murilobdo@admin.com.br",
                PasswordHash = Argon2.Hash("123"),
                JobRole = "Developer",
                CompanyId = adminCompany.CompanyId,
                Comission = 0,
                IsActive = true
            });

            modelBuilder.ApplyConfiguration(new CompanyMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new EmployeeMapping());
            modelBuilder.ApplyConfiguration(new CommandMapping());
            modelBuilder.ApplyConfiguration(new CommandItemMapping());
            modelBuilder.ApplyConfiguration(new ExpensesMappgin());
            modelBuilder.ApplyConfiguration(new ExpensesTypeMappgin());

            base.OnModelCreating(modelBuilder);
        }
    }
}
