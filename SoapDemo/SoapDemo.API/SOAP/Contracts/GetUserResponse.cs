using System.Runtime.Serialization;

namespace SoapDemo.API.SOAP.Contracts;

[DataContract(Name = "GetUserResponse", Namespace = "http://soapdemo.com/user-service/v1")]
public class GetUserResponse
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; } = string.Empty;
}
