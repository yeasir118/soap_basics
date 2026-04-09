using Microsoft.Extensions.Options;
using SoapWrapper.API.Middlewares;
using SoapWrapper.Application.Interfaces;
using SoapWrapper.Application.Services;
using SoapWrapper.Infrastructure.SOAP.Config;
using SoapWrapper.Infrastructure.SOAP.Wrappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configuration
builder.Services.Configure<SoapOptions>(
    builder.Configuration.GetSection("Soap"));

// DI
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserSoap, UserSoapWrapper>();

builder.Services.AddScoped<UserSoapServiceClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<SoapOptions>>().Value;

    var binding = SoapClientFactory.CreateBinding(options);
    var endpoint = SoapClientFactory.CreateEndpoint(options);

    return new UserSoapServiceClient(binding, endpoint);
});

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

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
