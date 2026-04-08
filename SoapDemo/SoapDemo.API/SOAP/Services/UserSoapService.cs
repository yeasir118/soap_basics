using System.ServiceModel;
using MapsterMapper;
using SoapDemo.API.SOAP.Contracts;
using SoapDemo.Application.Services;

namespace SoapDemo.API.SOAP.Services;

public class UserSoapService : IUserSoapService
{
    private readonly UserService _service;
    private readonly IMapper _mapper;

    public UserSoapService(UserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        var user = await _service.GetUserByIdAsync(request.Id);

        Console.WriteLine($"here: {user}");

        if (user == null)
            throw new FaultException($"User with Id {request.Id} not found.");

        return _mapper.Map<GetUserResponse>(user);
    }
}
