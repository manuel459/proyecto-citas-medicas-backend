using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Request
{
    public class DiagnosticoRequestDto
    {
        public int dni { get; set; }
    }

    public class DiagnosticoRequestPdfDto
    {
        public int idCita { get; set; }
        public int DniPaciente { get; set; }
        public string Codes { get; set; }
        public string Codmed { get; set; }
        public DateTime fecct { get; set; }
        public string diagnostico { get; set; }
        public string medicamentos { get; set; }

    }
}
