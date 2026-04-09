using SoapWrapper.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace SoapWrapper.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ExternalServiceTimeoutException ex)
        {
            await Handle(context, HttpStatusCode.GatewayTimeout, ex.Message);
        }
        catch (ExternalServiceUnavailableException ex)
        {
            await Handle(context, HttpStatusCode.BadGateway, ex.Message);
        }
        catch (ExternalServiceException ex)
        {
            await Handle(context, HttpStatusCode.BadGateway, ex.Message);
        }
        catch (Exception ex)
        {
            await Handle(context, HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public static async Task Handle(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
