namespace Consulta_medica.Dto.Response
{
    public class PacienteResponseDto
    {
        public int Id { get; set; }
        public int? Dnip { get; set; }
        public string Idtip { get; set; }
        public string Nomp { get; set; }
        public string Apellidos { get; set; }
        public int? Numero { get; set; }
        public int? Edad { get; set; }
        public string correoElectronico { get; set; }
        public int nTotal_Historias { get; set; }
    }
}
