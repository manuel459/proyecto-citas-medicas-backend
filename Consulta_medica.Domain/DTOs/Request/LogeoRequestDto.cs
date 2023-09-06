using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Dto.Request
{
    public class LogeoRequestDto
    {
        [Required]
        public string? Contraseña { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }
    }
}
