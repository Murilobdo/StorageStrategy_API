using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StorageStrategy.Data.Mappings;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Data.Context
{
    public class StorageDbContext : DbContext
    {
        private readonly IOptions<AppSettings> _appSettings;
        
        public StorageDbContext(DbContextOptions<StorageDbContext> options, IOptions<AppSettings> appSettings) : base(options)
        {
            _appSettings = appSettings;
        }

        public DbSet<ClientEntity> Client { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<CompanyEntity> Company { get; set; }
        public DbSet<EmployeeEntity> Employee { get; set; }
        public DbSet<CommandEntity> Command { get; set; }
        public DbSet<CommandItemEntity> CommandItems { get; set; }
        public DbSet<ExpenseEntity> Expenses { get; set; }
        public DbSet<ExpensesTypeEntity> ExpensesType { get; set; }
        public DbSet<StockHistoryEntity> StockHistory { get; set; }
        public DbSet<StockHistoryItemEntity> StockHistoryItems { get; set; }
        public DbSet<PaymentMethodEntity> PaymentMethod { get; set; }
        public DbSet<LogApp> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {   
            options.UseSqlServer(_appSettings.Value.Database);
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CompanyEntity adminCompany = new(companyId: 1, name: "Admin Company", description: "Admin", 
                createAt: DateTime.Now, validate: DateTime.Now.AddYears(10));

            modelBuilder.Entity<CompanyEntity>().HasData(adminCompany);

            modelBuilder.Entity<EmployeeEntity>().HasData(new EmployeeEntity {
                EmployeeId = 1,
                Name = "Murilo Bernardes (Admin)",
                Email = "murilobdo@admin.com",
                PasswordHash = Argon2.Hash("fib"),
                JobRole = EmployeeRole.Admin,
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
            modelBuilder.ApplyConfiguration(new ExpenseMappgin());
            modelBuilder.ApplyConfiguration(new ExpensesTypeMappgin());
            modelBuilder.ApplyConfiguration(new StockHistoryMapping());
            modelBuilder.ApplyConfiguration(new StockHistoryItemMapping());
            modelBuilder.ApplyConfiguration(new LogMapping());
            modelBuilder.ApplyConfiguration(new PaymentMapping());
            modelBuilder.ApplyConfiguration(new ClientMapping());
            modelBuilder.ApplyConfiguration(new PaymentMethodMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
