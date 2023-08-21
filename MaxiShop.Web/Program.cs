using MaxiShop.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MaxiShop.Application;
using MaxiShop.Infrastructure;
using MaxiShop.Application.Common;
using Microsoft.AspNetCore.Identity;
using MaxiShop.Infrastructure.Common;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using FluentValidation.AspNetCore;
using System.Reflection;
using MaxiShop.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy",x=>x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);

    if (context.HostingEnvironment.IsProduction() == false)
    {
        config.WriteTo.Console();
    }
});

#region Database Connectivity

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

#endregion

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Jwt Authorization header using the Bearer Scheme.
                        Enter 'Bearer' [space] and then your token in the input below.
                        Example:'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id ="Bearer"
                },
                Scheme = "Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


#region Configuration for Seeding Data to Database

static async void UpdateDatabaseAsync(IHost host)
{

    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }

            await SeedData.SeedDataAsync(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }

}

#endregion

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

var serviceProvider = app.Services;

await SeedData.SeedRoles(serviceProvider);

UpdateDatabaseAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CustomPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
