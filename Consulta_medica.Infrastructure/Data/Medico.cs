using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Consulta_medica.Models
{
    public partial class Medico
    {
        public string Codmed { get; set; }
        public string Codes { get; set; }
        public string Idtip { get; set; }
        public string Nombre { get; set; }
        public string sApellidos { get; set; }
        public string Sexo { get; set; }
        public DateTime? Nac { get; set; }
        public string Correo { get; set; }
        public string Pswd { get; set; }
        public int? Dni { get; set; }
        public string Idhor { get; set; }
        public string Asis { get; set; }
        public string sBodyHorario { get; set; }
        public int nEstado { get; set; }
    }
}
