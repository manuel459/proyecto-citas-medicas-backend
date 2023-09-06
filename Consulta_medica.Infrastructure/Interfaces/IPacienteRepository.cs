using Consulta_medica.Dto.Request;
using Consulta_medica.Models;

namespace Consulta_medica.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetPacientes(RequestGenericFilter request, string correoElectronico);
        Task<bool> AddPaciente(PacienteRequestDto request);
        Task<bool> UpdatePaciente(PacienteRequestDto request);
        Task<bool> DeletePaciente(int id);
        Task<IEnumerable<Paciente>> getPacientes();
    }
}
