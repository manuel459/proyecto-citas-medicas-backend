using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class CitasMedicasReporteResponse
    {
        public string? id { get; set; }
        public string NombreMedico { get; set; }
        public string NombrePaciente { get; set; }
        public DateTime? FechaCita { get; set; }
        public int? nCodigo { get; set; }
        public string EstadoCita { get; set; }
        public decimal Costo { get; set; }
    }
}
