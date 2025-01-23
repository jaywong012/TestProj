using Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Configurations.IntegrationTest;

public class InitConfigModel
{
    public required HttpClient Client { get; init; }
    public required NewProjectDbContext Context { get; init; }
    public required WebApplicationFactory<Program> Factory { get; init; }

    public void Dispose()
    {
        Client.Dispose();
        Context.Dispose();
        Factory.Dispose();
    }
}

public static class InitConfigs
{
    public static InitConfigModel SetupInMemoryDatabase()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");

                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NewProjectDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }


                    services.AddDbContext<NewProjectDbContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));
                });
            });

        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost:44301/"),
            HandleCookies = false,
            AllowAutoRedirect = false
        });

        var dbContextOptions =
            new DbContextOptionsBuilder<NewProjectDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        var context = new NewProjectDbContext(dbContextOptions);

        return new InitConfigModel
        {
            Client = client,
            Context = context,
            Factory = factory
        };
    }
}