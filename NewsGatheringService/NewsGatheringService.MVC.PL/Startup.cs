using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsCollector.BLL.Helpers;
using NewsCollector.BLL.Interfaces;
using NewsCollector.BLL.NewsParsers;
using NewsCollector.BLL.Services;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.BLL.Services;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Models;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsGatheringService.UOW.DAL.Repositories;
using Serilog;
using System;
using System.Text;

namespace NewsGatheringService.MVC.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<NewsAggregatorContext>(options =>
                options.UseSqlServer(defaultConnection));
                

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Welcome");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Welcome");
                });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            
            services.Configure<AppSettings>(appSettingsSection);

            services.AddScoped<IRepository<News>, NewsRepository>();
            
            services.AddScoped<IRepository<NewsStructure>, NewsStructureRepository>();
            
            services.AddScoped<IRepository<NewsUrl>, NewsUrlRepository>();
            
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            
            services.AddScoped<IRepository<Subcategory>, SubcategoryRepository>();
            
            services.AddScoped<IRepository<User>, UserRepository>();
            
            services.AddScoped<IRepository<Role>, RoleRepository>();
            
            services.AddScoped<IRepository<UserRole>, UserRoleRepository>();
            
            services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();

            services.AddScoped<INewsEvaluation, NewsEvaluation>();
           
            services.AddScoped<INewsTextLemmatization, NewsTextLemmatization>();
           
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<INewsService, NewsService>();
            
            services.AddScoped<IUserService, UserService>();
            
            services.AddTransient<IRssReader, RssReader>();
            
            services.AddTransient<IOnlinerNewsParser, OnlinerNewsParser>();
            
            services.AddTransient<Is13NewsParser, S13NewsParser>();
            
            services.AddTransient<ITutByNewsParser, TutByNewsParser>();
            
            services.AddScoped<IAddRecentNewsJob, AddRecentNewsJob>();
            
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());
            
            services.AddHangfireServer();
            
            services.AddControllersWithViews();
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider
            )
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            app.UseHangfireDashboard();
            
            /*recurringJobManager.AddOrUpdate(
                            "Run every hour",
                            () => serviceProvider.GetService<IAddRecentNewsJob>().AddUrlsNewsToDb(),
                            "0 * * * *"
                            );*/

            RecurringJob.AddOrUpdate(
                () => serviceProvider.GetService<IAddRecentNewsJob>().AddUrlsNewsToDb(),
                Cron.Hourly);
            
            RecurringJob.AddOrUpdate(
                () => serviceProvider.GetService<INewsService>().ParseNewsAndInsertIntoDb(),
#pragma warning disable CS0618 // Type or member is obsolete
                Cron.HourInterval(2));
#pragma warning restore CS0618 // Type or member is obsolete

            RecurringJob.AddOrUpdate(
                () => serviceProvider.GetService<INewsService>().PerformNewsEvaluationAsync(),
                Cron.Daily);

        }
    }
}
