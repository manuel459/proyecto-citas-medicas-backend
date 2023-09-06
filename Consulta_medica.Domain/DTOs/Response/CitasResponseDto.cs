using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class CitasResponseDto
    {
        public string Id { get; set; }
        public int? Dnip { get; set; }
        public string Codmed { get; set; }
        public DateTime? Feccit { get; set; }
        public string Estado { get; set; }
        public TimeSpan Hora { get; set; }
        public string Codes { get; set; }
        public string NombreMedico { get; set; }
        public string Especialidad { get; set; }
    }
}
