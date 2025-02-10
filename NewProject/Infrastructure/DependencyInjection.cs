using System.Text;
using System.Threading.RateLimiting;
using Domain.Common.Constants;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(CommonConstants.API_V1, new OpenApiInfo { Title = "My API", Version = CommonConstants.API_V1 });

            c.AddSecurityDefinition(CommonConstants.BEARER, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = configuration["JwtSettings:Key"],
                Name = CommonConstants.AUTHORIZATION,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = CommonConstants.BEARER
            });

            // Set the security requirements
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = CommonConstants.BEARER
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddConfigurationWebHost(this WebApplicationBuilder builder)
    {
        var isContainerized = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        if (isContainerized)
        {
            builder.WebHost.UseKestrel((options) =>
            {
                options.ListenAnyIP(7124, listenOptions =>
                {
                    listenOptions.UseHttps("/root/.aspnet/https/NewProject.pfx", "Test123");
                });
            });
        }
    }

    public static void AddInfrastructureConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        if (environment != CommonConstants.TEST_ENVIRONMENT)
        {
            services.AddDbContext<NewProjectDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void AddRateLimiterConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("IpPolicy", context =>
                RateLimitPartition.Get(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    })));
        });
    }

    public static void AddAuthenticateConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var jwtKey = configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Token invalid: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });
    }
}