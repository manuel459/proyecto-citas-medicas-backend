using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class MedicoResponseDto
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
    }
}
