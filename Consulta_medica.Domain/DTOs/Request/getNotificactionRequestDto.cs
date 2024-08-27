using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulta_medica.Domain.DTOs.Request
{
    public class getNotificactionRequestDto
    {
        public string? id_user { get; set; }
        public string? id_rol { get; set; }
    }
}
