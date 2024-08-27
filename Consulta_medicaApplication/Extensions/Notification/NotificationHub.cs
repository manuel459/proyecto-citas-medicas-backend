using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Consulta_medica.Extensions.Notification
{
    public class NotificationHub: Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveNotification", message);
        }
    }
}
