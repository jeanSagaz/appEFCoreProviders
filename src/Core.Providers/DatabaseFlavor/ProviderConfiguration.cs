using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Core.Providers.DatabaseFlavor;

public class ProviderConfiguration
{
    private readonly string _connectionString;
    public ProviderConfiguration With() => this;
    private static readonly string MigrationAssembly = typeof(ProviderConfiguration).GetTypeInfo().Assembly.GetName().Name;

    public static ProviderConfiguration Build(string connString)
    {
        return new ProviderConfiguration(connString);
    }

    public ProviderConfiguration(string connString)
    {
        _connectionString = connString;
    }

    public Action<DbContextOptionsBuilder> SqlServer =>
        //options => options.UseSqlServer(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        options => options.UseSqlServer(_connectionString);

    public Action<DbContextOptionsBuilder> MySql =>
        //options => options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString), sql => sql.MigrationsAssembly(MigrationAssembly));
        options => options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString), sql => sql.SchemaBehavior(MySqlSchemaBehavior.Ignore));

    public Action<DbContextOptionsBuilder> Postgre =>
        options =>
        {
            //options.UseNpgsql(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
            options.UseNpgsql(_connectionString);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        };

    public Action<DbContextOptionsBuilder> Sqlite =>
        //options => options.UseSqlite(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        options => options.UseSqlite(_connectionString);


    /// <summary>
    /// it's just a tuple. Returns 2 parameters.
    /// Trying to improve readability at ConfigureServices
    /// </summary>
    public static (DatabaseType, string) DetectDatabase(IConfiguration configuration) => (
        configuration.GetValue<DatabaseType>("AppSettings:DatabaseType", DatabaseType.None),
        configuration.GetConnectionString("DefaultConnection"));
}