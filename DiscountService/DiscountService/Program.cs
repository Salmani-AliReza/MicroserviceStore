using DiscountService.Infrastructure.Contexts;
using DiscountService.Infrastructure.MappingProfile;
using DiscountService.Model.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DiscountDataBaseContext>(
    o => o.UseSqlServer(builder.Configuration["DiscountConnection"])
);

builder.Services.AddAutoMapper(cfg => { }, typeof(DiscountMappingProfile).Assembly);

builder.Services.AddTransient<IDiscountService, DiscountService.Model.Services.DiscountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
