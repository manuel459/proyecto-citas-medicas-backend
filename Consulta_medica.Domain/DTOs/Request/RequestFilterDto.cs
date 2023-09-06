using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Request
{
    public class RequestFilterDto
    {
        public string texto { get; set; }
        public string status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }

    public class RequestFilterDtoNew 
    {
        public string? sNombreMedico { get; set; }
        public string? sNombrePaciente { get; set; }
        public DateTime? dFechaInicio { get; set; }
        public DateTime? dFechaFin { get; set; }
        public int? nStateFilter { get; set; }
    }
  
}
