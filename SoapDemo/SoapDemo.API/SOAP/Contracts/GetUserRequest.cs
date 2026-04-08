using System.Runtime.Serialization;

namespace SoapDemo.API.SOAP.Contracts;

[DataContract(Name = "GetUserRequest", Namespace = "http://soapdemo.com/user-service/v1")]
public class GetUserRequest
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
}
