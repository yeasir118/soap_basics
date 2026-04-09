using System.Xml.Linq;

namespace SoapDemo.API.SOAP.Auth;

public class SoapAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public SoapAuthMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();

        string body;
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        XNamespace authNs = "http://soapdemo.com/auth";
        var xml = XDocument.Parse(body);
        var apiKeyElement = xml.Descendants(authNs + "ApiKey").FirstOrDefault();

        if (apiKeyElement == null)
        {
            await WriteSoapFault(context, "Missing API Key");
            return;
        }

        var expectedKey = _config["SoapAuth:ApiKey"];
        if (apiKeyElement.Value != expectedKey)
        {
            await WriteSoapFault(context, "Invalid API Key");
            return;
        }

        await _next(context);
    }

    private static async Task WriteSoapFault(HttpContext context, string message)
    {
        context.Response.ContentType = "text/xml; charset=utf-8";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(
            $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            $"<s:Body><s:Fault><faultcode>s:Sender</faultcode><faultstring>{message}</faultstring></s:Fault></s:Body>" +
            $"</s:Envelope>");
    }
}
