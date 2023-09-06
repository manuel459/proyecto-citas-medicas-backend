namespace Consulta_medica.Dto.Request
{
    public class PacienteRequestDto
    {
        public int Id { get; set; }
        public int? Dnip { get; set; }
        public string Idtip { get; set; }
        public string Nomp { get; set; }
        public string Apellidos { get; set; }
        public int Numero { get; set; }
        public int? edad { get; set; }
        public string correoElectronico { get; set; }
    }
}
