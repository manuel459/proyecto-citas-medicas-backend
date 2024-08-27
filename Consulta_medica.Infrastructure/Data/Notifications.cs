using Consulta_medica.Models;

namespace Consulta_medica.Infrastructure.Data
{
    public class Notifications
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public int? State { get; set; }
        // Datos del receptor
        public string? id_rol_receptor { get; set; } 
        public int? id_user_receptor { get; set; } 
        public string? id_medico_receptor { get; set; } 
        // Datos del emisor
        public string? id_rol_emisor { get; set; }
        public int? id_user_emisor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        // Datos del receptor
        public Usuarios UserReceptor { get; set; }
        public Medico MedicoReceptor { get; set; }
        public Tipousuario TipoUsuarioReceptor { get; set; }

        // Datos del emisor
        public Usuarios UserEmisor { get; set; }
        public Tipousuario TipoUsuarioEmisor { get; set; }
    }
}
