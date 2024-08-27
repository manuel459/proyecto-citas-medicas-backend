using AutoMapper;
using Consulta_medica.Application.Interfaces;
using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Dto.Response;
using Consulta_medica.Extensions.Notification;
using Consulta_medica.Infrastructure.Data;
using Consulta_medica.Infrastructure.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Consulta_medica.Application.Services
{
    public class NotificationService: INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IMapper mapper, IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<Response> getNotification(getNotificactionRequestDto request) 
        {
            Response response = new Response();
            try
            {
                var list = await _unitOfWork.Notification.getNotification(request);
                response.exito = 1;
                response.mensaje = "listado extraido";
                response.data = list; 
            }
            catch (Exception ex)
            {
                response.mensaje = ex.Message;
            }
            return response;
           
        }

        public async Task<bool> sendNotificaction(NotificationRequestDto request) 
        {
            var result = await addNotification(request);
            if (result.exito == 1)
            {
                string notificationJson = JsonConvert.SerializeObject(result.data);

                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationJson);

                return true;
            }
            return false;
        }

        public async Task<Response> addNotification(NotificationRequestDto request) 
        {
            Response orespuesta = new Response();
            try
            {
                var createNotification = _mapper.Map<Notifications>(request);

                var resultCreateNotification = await _unitOfWork.Notification.addNotification(createNotification);

                var response = await _unitOfWork.Notification.getById(resultCreateNotification.Id);

                orespuesta.mensaje = "exito";
                orespuesta.exito = 1;
                orespuesta.data = response;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
                orespuesta.exito = 0;
            }
            
            return orespuesta;
        }
    }
}
