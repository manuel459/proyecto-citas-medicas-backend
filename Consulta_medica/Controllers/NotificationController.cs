using Consulta_medica.Application.Interfaces;
using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Extensions.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub> hubContext) 
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> getNotification([FromQuery] getNotificactionRequestDto request)
        {
            var response = await _notificationService.getNotification(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> sendNotification([FromBody] NotificationRequestDto request) 
        {
            var response = await _notificationService.sendNotificaction(request);
            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Hola mundo");
            return Ok();
        }
    }
}
