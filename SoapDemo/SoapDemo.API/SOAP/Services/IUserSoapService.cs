using SoapDemo.API.SOAP.Contracts;
using System.ServiceModel;

namespace SoapDemo.API.SOAP.Services;

[ServiceContract(Namespace = "http://soapdemo.com/user-service/v1")]
public interface IUserSoapService
{
    [OperationContract]
    Task<GetUserResponse> GetUser(GetUserRequest request);
}
