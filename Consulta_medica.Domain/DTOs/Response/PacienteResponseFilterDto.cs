using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class PacienteResponseFilterDto
    {
       
        public class List<T>
        {
            public int? Dnip { get; set; }
            public string Idtip { get; set; }
            public string Nomp { get; set; }
            public int? Numero { get; set; }
        }
    }

  
}
