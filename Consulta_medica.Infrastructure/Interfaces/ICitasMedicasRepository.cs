using Consulta_medica.Dto.Request;
using Consulta_medica.Models;

namespace Consulta_medica.Interfaces
{
    public interface ICitasMedicasRepository
    {
        Task<IEnumerable<CitasQueryDto>> GetCitas(RequestGenericFilter request, string usuario);
        Task<(Citas, bool)> AddCitas(CitasRequestDto request);
        Task<bool> UpdateCitas(CitasRequestDto request);
        Task<bool> DeleteCitas(int id);
        Task<Horarios> consultarHorario(DatosRequestCitasDto request);
        Task<IEnumerable<TimeSpan>> citasRegistradas(DatosRequestCitasDto request);
        Task<List<HistoriaMedica>> obtenerHistoriaMedica(int dnip);
        Task<IEnumerable<CitasRequestDto>> getCitasPending(DateTime fechaMañana);
        Task<IEnumerable<CitasPendientesMedico>> getCitasPendingByCodmed(DateTime FechaPrevia, string Codmed);
    }
}
