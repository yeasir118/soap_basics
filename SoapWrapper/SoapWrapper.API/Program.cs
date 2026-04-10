using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using SoapWrapper.API.Middlewares;
using SoapWrapper.Application.Interfaces;
using SoapWrapper.Application.Services;
using SoapWrapper.Infrastructure.Resilience;
using SoapWrapper.Infrastructure.Resilience.Config;
using SoapWrapper.Infrastructure.SOAP.Config;
using SoapWrapper.Infrastructure.SOAP.Wrappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configuration
builder.Services.Configure<SoapOptions>(
    builder.Configuration.GetSection("Soap"));

builder.Services.Configure<PollyOptions>(
    builder.Configuration.GetSection("Polly"));

builder.Services.Configure<SoapAuthOptions>(
    builder.Configuration.GetSection("SoapAuth"));

// Polly Policies
builder.Services.AddSingleton<AsyncRetryPolicy>(sp =>
{
    var options = sp.GetRequiredService<IOptions<PollyOptions>>().Value;
    return PollyPolicies.GetRetryPolicy(options);
});

builder.Services.AddSingleton<IAsyncPolicy>(sp =>
{
    var options = sp.GetRequiredService<IOptions<PollyOptions>>().Value;
    return PollyPolicies.GetCombinedPolicy(options);
});

// DI
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserSoap, UserSoapWrapper>(sp =>
{
    var client = sp.GetRequiredService<UserSoapServiceClient>();
    var retryPolicy = sp.GetRequiredService<IAsyncPolicy>();
    var authOptions = sp.GetRequiredService<IOptions<SoapAuthOptions>>().Value;

    return new UserSoapWrapper(client, retryPolicy, authOptions);
});

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
