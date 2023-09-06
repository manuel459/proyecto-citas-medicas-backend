using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Consulta_medica.Models
{
    public partial class Especialidad
    {
        public string Codes { get; set; }
        public string Nombre { get; set; }
        public decimal? Costo { get; set; }
    }
}
