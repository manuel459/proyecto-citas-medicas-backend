using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Infrastructure.Data
{
    public class Permisos_Roles
    {
        [Key]
        public int id { get; set; }
        public string idtip { get; set; }
        public int nId_Permiso { get; set; }
        public int nEstado { get; set; }
        public int nUser_Create { get; set; }
        public DateTime dCreate_Datetime { get; set; }
        public int nUser_Update { get; set; }
        public DateTime dUpdate_Datetime { get; set; }
    }
}
