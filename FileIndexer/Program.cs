using FileIndexer.Classes;
using FileIndexer.Models;
using FileIndexer.Repositories;
using FileIndexer.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FileIndexer;

static class Program
{
    private static IServiceProvider ServiceProvider { get; }
    private static IConfiguration Configuration { get; }

    static Program()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    [STAThread]
    static void Main()
    {
        var indexerService = ServiceProvider.GetRequiredService<IIndexerService>();

        indexerService.IndexFiles();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(
            configure => configure.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "hh:mm:ss ";
            }));
        services.Configure<IndexerSettings>(Configuration.GetSection("IndexerSettings"));
        var dbSettings = Configuration.GetSection("DatabaseConnections").Get<DatabaseSettings>();
        services.AddOptions();

        services.AddTransient<IIndexerService, IndexerService>();
        services.AddTransient<IRepository<MediaFile>, Repository<MediaFile>>();
        services.AddTransient<IRepository<MediaFileTag>, Repository<MediaFileTag>>();
        services.AddTransient<TagRepository, TagRepository>();
        services.AddTransient<IDbConnection>(db => new SqliteConnection(dbSettings.ConnectionString));
        services.AddTransient<IStringTokenizer, StringTokenizer>();
    }
}