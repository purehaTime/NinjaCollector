using DbService.Interfaces;

namespace DbService.Services
{
    public class DbConfiguration : IDbConfiguration
    {
        public string DatabaseName { get; }

        public DbConfiguration(IConfiguration config)
        {
            var dbName = config.GetSection("MongoSetup:DbName").Value;
            DatabaseName =  string.IsNullOrEmpty(dbName) ? "ninja_collector" : dbName;
        }
    }
}
