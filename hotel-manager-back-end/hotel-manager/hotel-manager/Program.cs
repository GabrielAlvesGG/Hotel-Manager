using hotel_manager.Data;
using hotel_manager.Repository;
using hotel_manager.Service;
using Microsoft.EntityFrameworkCore;
using hotel_manager.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularLocal", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .WithMethods("POST")
            .AllowAnyHeader();
    });
});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "A conexão DefaultConnection não foi configurada.");

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<HotelRepository>();
builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<ClientService>();

builder.Services.AddScoped<IPasswordHasher<Client>, PasswordHasher<Client>>();



var app = builder.Build();


app.UseRouting();

app.UseCors("AngularLocal");

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
