using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class UserResponseDto
    {
        public string CorreoElectronico { get; set; }
        public string Token { get; set; }
        public string? Nombre { get; set; }
        public string? Idtip { get; set; }
    }
}
