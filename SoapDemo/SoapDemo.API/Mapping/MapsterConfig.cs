using Mapster;
using SoapDemo.API.SOAP.Contracts;
using SoapDemo.Application.Entities;

namespace SoapDemo.API.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, GetUserResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .IgnoreNonMapped(true);
    }
}
