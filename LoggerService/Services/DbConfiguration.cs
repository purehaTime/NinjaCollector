using LoggerService.Interfaces;

namespace LoggerService.Services
{
    public class DbConfiguration : IDbConfiguration
    {
        private string _pass;
        private string _path;


        public DbConfiguration(IConfiguration appConfig)
        {
            _path = appConfig.GetSection("LiteDb:FileName")?.Value ?? "log.data";
            _pass = appConfig.GetSection("LiteDb:Password")?.Value ?? "ninjapass";
        }

        public string GetConnectionString()
        {
            return $"Filename={_path};Connection=shared;Password={_pass}";
        }
    }
}
