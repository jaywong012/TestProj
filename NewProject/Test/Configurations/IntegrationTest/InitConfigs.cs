using Application.Features.Logins.Commands;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Test.Configurations.IntegrationTest;

public class InitConfigModel
{
    public required HttpClient Client { get; set; }
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
        SeedDatabase.SeedAccounts(context);

        return new InitConfigModel
        {
            Client = client,
            Context = context,
            Factory = factory
        };
    }

    public static async Task<HttpClient> GenerateToken(HttpClient client)
    {
        var loginRequest = new GenerateJwtTokenCommandRequest
        {
            UserName = "jac",
            Password = "123"
        };

        var jsonContent = Utilities.SerializeToJsonContent(loginRequest);
        var loginResponse = await client.PostAsync("api/Login", jsonContent);
        loginResponse.EnsureSuccessStatusCode();
        var jwtToken = loginResponse.Content.ReadAsStringAsync().Result;

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        return client;
    }
}