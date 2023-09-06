using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Request
{
    public class usuario
    {
        public string? correoElectronico { get; set; }
        public string? token { get; set; }
        public string? nombre { get; set; }
    }
}
