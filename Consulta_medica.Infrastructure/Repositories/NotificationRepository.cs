using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Infrastructure.Data;
using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Models;
using Microsoft.EntityFrameworkCore;

namespace Consulta_medica.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly consulta_medicaContext _context;
        public NotificationRepository(consulta_medicaContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<getNotificationResponseDto>> getNotification(getNotificactionRequestDto request) 
        {
            var query = _context.Notifications
                        .Include(x => x.UserReceptor)
                        .Include(x => x.MedicoReceptor)
                        .Include(x => x.TipoUsuarioReceptor)
                        .Include(x => x.UserEmisor)
                        .Include(x => x.TipoUsuarioEmisor)
                        .OrderByDescending(x => x.Id)
                        .AsQueryable();

            switch (request.id_rol)
            {
                // MEDDICO
                case "U002":
                    query = query.Where(x => x.id_medico_receptor == request.id_user);
                    break;
                default:
                    query = query.Where(x => x.id_user_receptor.ToString() == request.id_user);
                    break;
            }
            var result = query.Select(x => 
                            new getNotificationResponseDto 
                            {
                                id = x.Id,
                                message = x.Message,
                                state = x.State,
                                id_rol_receptor = x.id_rol_receptor,
                                nombre_rol_receptor = x.TipoUsuarioReceptor.Nomtip,
                                id_user_receptor = x.id_user_receptor,
                                nombre_user_receptor = x.UserReceptor.sNombres + " " + x.UserReceptor.sApellidos,
                                id_medico_receptor = x.id_medico_receptor,
                                nombre_medico_receptor = x.MedicoReceptor.Nombre + " " + x.MedicoReceptor.sApellidos,
                                id_user_emisor = x.id_user_emisor,
                                nombre_user_emisor = x.UserEmisor.sNombres + " " + x.UserEmisor.sApellidos,
                                id_rol_emisor = x.id_rol_emisor,
                                nombre_rol_emisor = x.TipoUsuarioEmisor.Nomtip,
                                createdAt = x.CreatedAt
                            }).ToList();

            return result;
        }

        public async Task<Notifications> addNotification(Notifications request) 
        {
            request.State = 1;
            _context.Notifications.Add(request);

            var id = await _context.SaveChangesAsync();

            return request;
        }

        public async Task<getNotificationResponseDto> getById(int id)
        {
            var query = _context.Notifications
                        .Include(x => x.UserReceptor)
                        .Include(x => x.MedicoReceptor)
                        .Include(x => x.TipoUsuarioReceptor)
                        .Include(x => x.UserEmisor)
                        .Include(x => x.TipoUsuarioEmisor)
                        .Where(x => x.Id == id)
                        .AsQueryable();

            var result = await query.Select(x =>
                            new getNotificationResponseDto
                            {
                                id = x.Id,
                                message = x.Message,
                                state = x.State,
                                id_rol_receptor = x.id_rol_receptor,
                                nombre_rol_receptor = x.TipoUsuarioReceptor.Nomtip,
                                id_user_receptor = x.id_user_receptor,
                                nombre_user_receptor = x.UserReceptor.sNombres + " " + x.UserReceptor.sApellidos,
                                id_medico_receptor = x.id_medico_receptor,
                                nombre_medico_receptor = x.MedicoReceptor.Nombre + " " + x.MedicoReceptor.sApellidos,
                                id_user_emisor = x.id_user_emisor,
                                nombre_user_emisor = x.UserEmisor.sNombres + " " + x.UserEmisor.sApellidos,
                                id_rol_emisor = x.id_rol_emisor,
                                nombre_rol_emisor = x.TipoUsuarioEmisor.Nomtip,
                                createdAt = x.CreatedAt
                            }).FirstAsync();

            return result;
        }
    }
}
