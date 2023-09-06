using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Domain.DTOs.Request
{
    public class restorePasswordRequestDto
    {
        [Required]
        public string sEmail { get; set; }
        [Required]
        public string sOldPassword { get; set; }
        [Required]
        public string sNewPassword { get; set; }
    }

    public class restoreUserDto 
    {
        public string? sEmail { get; set; }
        public string? sPswd { get; set; }
        public string? sRol { get; set; }
    }
}
