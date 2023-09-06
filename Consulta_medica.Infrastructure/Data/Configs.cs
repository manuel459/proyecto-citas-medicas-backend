using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Models
{
    public class Configs
    {
        [Key]
        public int id { get; set; }
        public string sTable { get; set; }
        public int nCodigo { get; set; }
        public string sDescription { get; set; }
    }
}
