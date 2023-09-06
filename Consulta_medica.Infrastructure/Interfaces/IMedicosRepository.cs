using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Models;

namespace Consulta_medica.Interfaces
{
    public interface IMedicosRepository
    {
        Task<IEnumerable<Medico>> GetMedicos(RequestGenericFilter request, string usuario);
        Task<(bool,string)> AddMedico(MedicoRequestDto request);
        Task<bool> UpdateMedico(MedicoRequestDto request);
        Task<bool> DeleteMedico(string id, int nEstado);
        Task<bool> UpdateBodyHorario(string Codmed, string bodyMessage);
        Task<Medico> BusinessHours(string Codmed);
        Task<IEnumerable<HorarioDto>> ObtenerInfoHorario(string Codmed);
        string getEmailMedico(string CodMed);
    }
}
