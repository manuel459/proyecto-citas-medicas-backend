namespace Consulta_medica.Dto.Request
{
    public class PagosRequestDto
    {
        public int Nro_Transaction { get; set; }
        public string Nro_Tarjeta { get; set; }
        public int Id { get; set; } //CODIGO DE CITA
        public int Dni_Pagador { get; set; }
        public string sNombre_Especialidad { get; set; }
        public string sNombre_Medico { get; set; }
        public string sNombre_Paciente { get; set; }
        public decimal dImporte_Total { get; set; }
    }
}
