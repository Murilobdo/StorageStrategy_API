using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StorageStrategy.Data.Mappings;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;
using System.Threading.Tasks;
using System.Linq;

namespace StorageStrategy.Data.Context
{
    public class StorageDbContext : DbContext, IDataProtectionKeyContext
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
        public DbSet<PixChargeEntity> PixCharges { get; set; }
        public DbSet<PixCredentialEntity> PixCredentials { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {   
            options.UseSqlServer(_appSettings.Value.Database);
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Removed HasData seeding for admin company and admin user.
            // Seeding is handled at runtime by PlatformAdminSeeder (Domain) so the company and user are created only if they don't exist.

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
            modelBuilder.ApplyConfiguration(new PixChargeMapping());
            modelBuilder.ApplyConfiguration(new PixCredentialMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
