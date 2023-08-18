using MaxiShop.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MaxiShop.Application;
using MaxiShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

#region Database Connectivity

builder.Services.AddDbContext<ApplicationDbContext>(
    options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
