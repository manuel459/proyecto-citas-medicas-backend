using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Infrastructure.Data;

namespace Consulta_medica.Infrastructure.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notifications> addNotification(Notifications request);
        Task<IEnumerable<getNotificationResponseDto>> getNotification(getNotificactionRequestDto request);
        Task<getNotificationResponseDto> getById(int id);
    }
}
