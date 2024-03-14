using GbStoreApi.Application.Interfaces;
using GbStoreApi.Application.Services;
using GbStoreApi.Data.Context;
using GbStoreApi.Data.Implementation;
using GbStoreApi.Domain.Dto;
using GbStoreApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MyConfigurationClass>(builder.Configuration.GetSection("Configuration"));
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(connectionString);
});

builder.Services.AddTransient<JwtRefreshExpiredMiddleware>();


// Add services to the container.
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<GbStoreApi.Domain.Repository.IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(x =>{
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>{
        x.SaveToken = true;
        var configuration = new MyConfigurationClass { PrivateKey = builder.Configuration.GetSection("Configuration").GetValue("PrivateKey", "") ?? "" };
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
        };
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:5173/")
            .AllowAnyMethod()
            .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "token")
            .WithExposedHeaders("token")
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true)
        );
}
 );

var app = builder.Build();

app.UseFactoryActivatedMiddleware();

var scope = app.Services.CreateScope();

scope.ServiceProvider.GetService<DataContext>()?.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtRefreshExpiredMiddleware>();

app.MapControllers();

app.Run();
