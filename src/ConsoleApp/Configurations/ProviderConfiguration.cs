namespace ConsoleApp.Configurations;

public record Provider(string Name, string Assembly)
{
    public static readonly Provider SqlServer = new(nameof(SqlServer), typeof(SqlServer.Marker).Assembly.GetName().Name!);
    public static readonly Provider MySql = new(nameof(MySql), typeof(MySql.Marker).Assembly.GetName().Name!);
}
