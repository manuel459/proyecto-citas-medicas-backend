
using System;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Consulta_medica.Models
{
    public partial class Citas
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int? Dnip { get; set; }
        public string? Codmed { get; set; }
        public DateTime? Feccit { get; set; }
        public int Estado { get; set; }
        public string? Codes { get; set; }
        public TimeSpan Hora { get; set; }
        public int nEstado_Pago { get; set; }
    }


}
