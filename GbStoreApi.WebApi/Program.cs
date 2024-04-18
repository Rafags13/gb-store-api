using Amazon.S3;
using GbStoreApi.Application;
using GbStoreApi.Application.Extensions;
using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Dto;
using GbStoreApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var corsName = "corsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MyConfigurationClass>(builder.Configuration.GetSection("Configuration"));

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(connectionString);
});

builder.Services.AddTransient<JwtRefreshExpiredMiddleware>();

builder.Services
    .AddUserServices()
    .AddProductServices()
    .AddFileServices()
    .AddDifferentUnitOfWork();

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerCustomConfiguration();

builder.Services.AddCustomDefaultAWSOptions(builder.Configuration);

builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     })
    .AddCustomJwtConfiguration(builder.Configuration);

builder.Services.AddCorsConfiguration(corsName);

var app = builder.Build();

app.UseFactoryActivatedMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsName);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();