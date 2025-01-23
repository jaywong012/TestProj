using Application;
using Application.Common;
using Domain.Base;
using Infrastructure;
using Infrastructure.CustomMiddleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigurationWebHost();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        p => p
            .WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithHeaders("Content-Type"));
});

builder.Services.AddRateLimiterConfiguration(builder.Configuration);
builder.Services.AddAuthenticateConfiguration(builder.Configuration);
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(Constants.ADMIN))
    .AddPolicy("UserPolicy", policy => policy.RequireRole(Constants.USER));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
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


public partial class Program { }