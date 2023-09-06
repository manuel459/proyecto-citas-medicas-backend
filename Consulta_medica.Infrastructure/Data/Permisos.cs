using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Models
{
    public class Permisos
    {
        [Key]
        public int nId { get; set; }
        public string? sSlug { get; set; }
        public string? sdescripcion { get; set; }
        public int nEstado { get; set; }
        public string? nUser_Create { get; set; }
        public DateTime dCreate_Datime { get; set; }
        public string? nUser_Update { get; set; }
        public DateTime dUpdate_Datime { get;}
    }
}
