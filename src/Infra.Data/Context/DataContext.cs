using Core.DomainObjects;
using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Infra.Data.Context
{
    public sealed class DataContext : DbContext, IUnitOfWork
    {
        private readonly ILogger<DataContext> _logger;

        public DataContext(ILogger<DataContext> logger,
            DbContextOptions<DataContext> options) : base(options)
        {
            _logger = logger;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public async Task<bool> Commit()
        {
            try
            {
                var success = await SaveChangesAsync() > 0;
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no banco de dados.");
            }

            return false;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Entity;
                //If the inserted object is an Auditable. 
                if (auditableEntity is not null)
                {
                    auditableEntity.DateCreated = DateTime.UtcNow;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Entity;
                if (auditableEntity is not null)
                {
                    auditableEntity.DateUpdated = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //const string connectionString = "Server=DESKTOP-TINECUL\\SQLEXPRESS;Database=EFCoreProvider;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=false";
            //optionsBuilder
            //    .UseSqlServer(connectionString)                
            //    //.UseMySql(connection)
            //    .LogTo(Console.WriteLine, LogLevel.Information)
            //    .EnableSensitiveDataLogging();

            //const string connectionString = "server=127.0.0.1;database=efcoreprovider;uid=root;pwd=";
            //optionsBuilder
            //    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), p => p.SchemaBehavior(MySqlSchemaBehavior.Ignore))
            //    .LogTo(Console.WriteLine, LogLevel.Information)
            //    .EnableSensitiveDataLogging()
            //    .EnableDetailedErrors();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
