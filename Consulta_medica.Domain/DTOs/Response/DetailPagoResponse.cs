using System;

namespace Consulta_medica.Dto.Response
{
    public class DetailPagoResponse
    {
        public int? nDnip { get; set; }
        public string sNombre_Paciente { get; set; }
        public string sEspecialidad { get; set; }
        public string sNombre_Medico { get; set; }
        public DateTime? dFecha_Cita { get; set; }
        public decimal? dImporte_Total { get; set; }
    }
}
