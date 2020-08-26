using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NewsGatheringService.Data.ContextDb
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<NewsAggregatorContext>
    {
        public NewsAggregatorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsAggregatorContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringServiceMVC\appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new NewsAggregatorContext(optionsBuilder.Options);
        }
    }
}
