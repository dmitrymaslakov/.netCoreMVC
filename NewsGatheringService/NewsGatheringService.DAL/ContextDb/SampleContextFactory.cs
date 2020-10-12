using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NewsGatheringService.DAL.Models;
using System;
using System.IO;

namespace NewsGatheringService.DAL.ContextDb
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<NewsAggregatorContext>
    {
        private const string APPSETTINGS_FILE_PATH = @"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.MVC.PL\appsettings.json";
        public NewsAggregatorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsAggregatorContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(APPSETTINGS_FILE_PATH);
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new NewsAggregatorContext(optionsBuilder.Options);
        }
    }
}
