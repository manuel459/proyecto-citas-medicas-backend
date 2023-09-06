using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Dto.Response
{
    public class Response
    {
        public int exito { get; set; } = 0;
        public string mensaje { get; set; }
        public object data { get; set; }
        public object Errors { get; set; }
    }
}
