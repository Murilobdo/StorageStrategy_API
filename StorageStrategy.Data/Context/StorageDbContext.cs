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
            options.UseSqlServer("Server=TI-17\\LOCALHOST;Initial Catalog=StorageStrategy; User Id=sa; Password=localdb; TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEntity>()
                .HasData(new CompanyEntity(companyId: 1, name: "Bar do Murps", description: "Bar", createAt: DateTime.Now),
                        new CompanyEntity(companyId: 2, name: "Rei do Baco", description: "Tabacaria", createAt: DateTime.Now));

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
