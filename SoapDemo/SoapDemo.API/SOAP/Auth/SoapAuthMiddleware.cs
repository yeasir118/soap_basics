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
        XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";
        var doc = new XDocument(
            new XElement(s + "Envelope",
                new XAttribute(XNamespace.Xmlns + "s", s.NamespaceName),
                new XElement(s + "Body",
                    new XElement(s + "Fault",
                        new XElement("faultcode", "s:Sender"),
                        new XElement("faultstring", message)
                    )
                )
            )
        );

        var sb = new System.Text.StringBuilder();
        var settings = new System.Xml.XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
        using (var writer = System.Xml.XmlWriter.Create(sb, settings))
            doc.Save(writer);

        context.Response.ContentType = "text/xml; charset=utf-8";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(sb.ToString());
    }
}
