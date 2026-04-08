using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SoapCore;
using SoapDemo.API.Mapping;
using SoapDemo.API.SOAP.Services;
using SoapDemo.Application.Interfaces;
using SoapDemo.Application.Services;
using SoapDemo.Infrastructure.Persistance;
using SoapDemo.Infrastructure.Persistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql("Host=localhost;Database=soap_demo;Username=postgres;Password=postgres"));

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// Mapster DI
MapsterConfig.RegisterMappings();

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// SOAP
builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserSoapService, UserSoapService>();

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

app.UseRouting();

app.UseAuthorization();

app.UseSoapEndpoint<IUserSoapService>(
    "/UserService.svc",
    new SoapEncoderOptions()
);

app.MapControllers();

app.Run();
