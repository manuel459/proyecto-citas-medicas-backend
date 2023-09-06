using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consulta_medica.Models
{
    public partial class Pagos
    {
        [Key]
        public int? nId_Pago { get; set; }
        public int sCod_Cita { get; set; }
        public string nNumero_Tarjeta { get; set; }
        public int? nMes { get; set; }
        public int? nAnio { get; set; }
        public int nDni { get; set; }
        public DateTime dCreate_Datetime { get; set; }
        public decimal dImporte_Total { get; set; }
        public string urlBase { get; set; } 
    }
}
