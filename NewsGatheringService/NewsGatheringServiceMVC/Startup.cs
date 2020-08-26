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
using NewsGatheringService.Data.Abstract;
using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;
using NewsGatheringService.Domain.Concrete;
using NewsGatheringService.Domain.Concrete.Repositories;
using Serilog;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string defaultConnection = Configuration.GetConnectionString("DefaultConnection");
            //string userConnection = Configuration.GetConnectionString("UserConnection");

            services.AddDbContext<NewsAggregatorContext>(options =>
                options.UseSqlServer(defaultConnection));

            /*services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(userConnection));*/


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Register");
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Welcome");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Welcome");
                });

            /*services.AddAuthorization(opts => {
                opts.AddPolicy("OnlyForLondon", policy => {
                    policy.RequireClaim(ClaimTypes.Locality, "Лондон", "London");
                });
                opts.AddPolicy("OnlyForMicrosoft", policy => {
                    policy.RequireClaim("company", "Microsoft");
                });
            });*/

            services.AddSingleton<IRepository, FakeRepository>();
            services.AddScoped<IRepository<News>, NewsRepository>();
            services.AddScoped<IRepository<NewsStructure>, NewsStructureRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Subcategory>, SubcategoryRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Role>, RoleRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsService>();

            services.AddTransient<IRssReader, RssReader>();
            services.AddTransient<IOnlinerNewsParser, OnlinerNewsParser>();
            services.AddTransient<Is13NewsParser, S13NewsParser>();
            services.AddTransient<ITutByNewsParser, TutByNewsParser>();
            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
