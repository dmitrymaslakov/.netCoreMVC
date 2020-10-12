using System;
using System.IO;
using System.Reflection;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsCollector.BLL.Helpers;
using NewsCollector.BLL.Interfaces;
using NewsCollector.BLL.NewsParsers;
using NewsCollector.BLL.Services;
using NewsGatheringService.BLL.DTO;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.BLL.Services;
using NewsGatheringService.DAL.ContextDb;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Repositories;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsGatheringService.DAL.Models;

namespace NewsGatheringServiceWebAPI
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
            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<NewsAggregatorContext>(options =>
                options.UseSqlServer(defaultConnection));

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            
            IMapper mapper = mapperConfig.CreateMapper();
            
            services.AddSingleton(mapper);

            services.AddMediatR(typeof(Startup));
            
            services.AddCors();
            
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(sa =>
            {
                sa.SwaggerDoc("v1", new OpenApiInfo() { Title = "NewsGatheringService WebAPI", Version = "v.1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                sa.IncludeXmlComments(xmlPath);
            });
            
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseSwagger();

            app.UseSwaggerUI(sa =>
            {
                sa.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
            });

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
