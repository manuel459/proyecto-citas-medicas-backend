namespace Consulta_medica.Domain.DTOs.Response
{
    public class UserListResponseDto
    {
        public int nIdUser { get; set; }
        public string nIptip { get; set; }
        public string sNomtip { get; set; }
        public string sNombres { get; set; }
        public string sApellidos { get; set; }
        public string sSexo { get; set; }
        public DateTime? dNac { get; set; }
        public string sCorreo { get; set; }
        public int? nDni { get; set; }
    }
}
