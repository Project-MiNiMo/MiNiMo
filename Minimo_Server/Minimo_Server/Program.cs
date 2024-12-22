using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;

namespace MinimoServer
{
    // Database Context
    public class GameDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Time> Time { get; set; }
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
        
        // Owns : Currency
        // OwnsMany : Buildings / Items
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(account => 
            {
                account.ToTable("Players");
                account.OwnsOne(a => a.Currency, currency =>
                {
                    currency.Property<int>("Id");
                    currency.HasKey("Id");
                });
                account.OwnsMany(a => a.Buildings, building =>
                {
                    building.WithOwner().HasForeignKey("AccountId");
                    building.Property<int>("Id");
                    building.HasKey("Id");
                });
                account.OwnsMany(a => a.Items, item =>
                {
                    item.WithOwner().HasForeignKey("AccountId");
                    item.Property<int>("Id");
                    item.HasKey("Id");
                });
            });
            modelBuilder.Entity<Time>().ToTable("Time");
        }

    }

    // Startup class to configure services and middleware
    public class Startup(IConfiguration configuration)
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GameDbContext>(options =>
                options.UseSqlite("Data Source=game.db"));
            services.AddControllers();
            services.AddScoped<TimeService>();
            services.AddScoped<JwtService>();
            services.AddSingleton<TableDataService>(new TableDataService());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minimo Server API", Version = "v1" });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // IgnoreAuthFilter 추가
                c.OperationFilter<IgnoreAuthFilter>();
            }); 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        // jwt:key
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimo Server API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // Program class to run the application
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<GameDbContext>();
                context.Database.EnsureCreated();
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5093");
                });
    }
}