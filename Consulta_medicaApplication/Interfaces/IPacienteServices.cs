using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;

namespace Consulta_medica.Application.Interfaces
{
    public interface IPacienteServices
    {
        Task<Response> GetPacientes(RequestGenericFilter request, string usuario);
        Task<Response> AddPaciente(PacienteRequestDto request);
        Task<Response> UpdatePaciente(PacienteRequestDto request);
        Task<Response> DeletePaciente(int id);
    }
}
