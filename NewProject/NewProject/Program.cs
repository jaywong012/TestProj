using Application;
using Application.Common;
using Domain.Base;
using Infrastructure;
using Infrastructure.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationWebHost();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        p => p
            .WithOrigins("http://localhost:8080", "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithHeaders("Content-Type"));
});

builder.Services.AddRateLimiterConfiguration();
builder.Services.AddAuthenticateConfiguration(builder.Configuration);
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(Constants.ADMIN))
    .AddPolicy("UserPolicy", policy => policy.RequireRole(Constants.USER));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureConfigurations(builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseRateLimiter();

app.UseErrorHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public abstract partial class Program { }