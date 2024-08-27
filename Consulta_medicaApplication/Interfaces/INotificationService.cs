using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Dto.Response;

namespace Consulta_medica.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> sendNotificaction(NotificationRequestDto request);
        Task<Response> addNotification(NotificationRequestDto request);
        Task<Response> getNotification(getNotificactionRequestDto request);
    }
}
