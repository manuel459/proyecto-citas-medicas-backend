using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Consulta_medica.Models
{
    public partial class Usuarios
    {
        [Key]
        public int nIdUser { get; set; }
        public string nIptip { get; set; }
        public string sNombres { get; set; }
        public string sApellidos { get; set; }
        public string sSexo { get; set; }
        public DateTime? dNac { get; set; }
        public string sCorreo { get; set; }
        public string sPswd { get; set; }
        public int? nDni { get; set; }
    }
}
