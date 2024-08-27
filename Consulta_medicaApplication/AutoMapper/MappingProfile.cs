using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Infrastructure.Data;
using Profile = AutoMapper.Profile;

namespace Consulta_medica.Application.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<NotificationRequestDto, Notifications>().ReverseMap();
            CreateMap<NotificationResponseDto,Notifications>().ReverseMap();
            // Agrega más mapeos según sea necesario
        }
    }
}
