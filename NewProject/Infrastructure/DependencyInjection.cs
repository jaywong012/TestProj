using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        //services
        //    .AddDbContext<NewProjectDbContext>(opts => opts
        //        .UseSqlServer(configuration
        //            .GetConnectionString("DefaultConnection")));
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Check if we are in the Test environment
        if (environment == "Test")
        {
            // Use InMemory Database for testing
            //services.AddDbContext<NewProjectDbContext>(options =>
            //    options.UseInMemoryDatabase("TestDatabase"));
        }
        else
        {
            // Use SQL Server in production or development environments
            services.AddDbContext<NewProjectDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }



        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}