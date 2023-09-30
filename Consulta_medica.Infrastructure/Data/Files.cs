using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Infrastructure.Data
{
    public class Files
    {
        [Key]
        public int nId_File { get; set; }
        public string? sEntidad { get; set; }
        public int nId_Entidad { get; set; }
        public string? sUrl { get; set; }
        public string? sType_File { get; set; }
        public string? sFile_Name { get; set; }
    }
}
