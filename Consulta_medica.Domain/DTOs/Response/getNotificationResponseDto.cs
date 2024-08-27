namespace Consulta_medica.Domain.DTOs.Response
{
    public class getNotificationResponseDto
    {
        public int id { get; set; }
        public string? message { get; set; }
        public int? state { get; set; }
        // Datos del Receptor
        public string? id_rol_receptor { get; set; }
        public string? nombre_rol_receptor { get; set; }
        public int? id_user_receptor { get; set; }
        public string? nombre_user_receptor { get; set; }
        public string? id_medico_receptor { get; set; }
        public string? nombre_medico_receptor { get; set; }
        // Datos del emisor
        public string? id_rol_emisor { get; set; }
        public string? nombre_rol_emisor { get; set; }
        public int? id_user_emisor { get; set; }
        public string? nombre_user_emisor { get; set; }
        public DateTime createdAt { get; set; }
    }
}
