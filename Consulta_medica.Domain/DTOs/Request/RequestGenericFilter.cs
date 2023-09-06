namespace Consulta_medica.Dto.Request
{
    public class RequestGenericFilter
    {
        public int? numFilter { get; set; } = null;
        public string? textFilter { get; set; } = null;
        public string? sFilterOne { get; set; } = null;
        public string? sFilterTwo { get; set; } = null;
        public string? dFechaInicio { get; set; } = null;
        public string? dFechaFin { get; set; } = null;
        public string? sCorreo { get; set; } = null;
    }
}
