using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Models
{
    public class HistorialMedico
    {
        [Key]
        public int Id { get; set; }
        public int idCita { get; set; }
        public string Codes { get; set; }
        public string Codmed { get; set; }
        public int Dnip { get; set; }
        public DateTime Fecct { get; set; }
        public string Diagnostico { get; set; }
        public string Receta { get; set; }
    }
}
