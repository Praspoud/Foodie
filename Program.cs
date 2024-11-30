using Foodie;
using Foodie.Common.Services;
using Foodie.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FoodieDbContext>(db =>
    db.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IServiceFactory, ServiceFactory>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddTransient<IRiddhaSessionAccessor, RiddhaSessionAccessor>();
builder.RegisterServices();
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

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RegisterApi();

app.Run();
