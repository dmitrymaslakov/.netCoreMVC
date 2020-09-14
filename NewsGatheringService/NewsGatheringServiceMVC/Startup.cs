using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsCollector.Abstract;
using NewsCollector.Concrete;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Concrete;
using NewsGatheringService.Domain.Concrete.Repositories;
using Serilog;
using System;
using System.Security.Claims;

namespace NewsGatheringServiceMVC
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

            services.AddScoped<IRepository<News>, NewsRepository>();
            services.AddScoped<IRepository<NewsStructure>, NewsStructureRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Subcategory>, SubcategoryRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Role>, RoleRepository>();
            services.AddScoped<IRepository<UserRole>, UserRoleRepository>();

            services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();

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
            IBackgroundJobClient backgroundJobClient, 
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
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
            recurringJobManager.AddOrUpdate(
                            "Run every minute",
                            () => serviceProvider.GetService<IAddRecentNewsJob>().AddNews(),
                            "* * * * *"
                            
                            );
        }
    }
}
