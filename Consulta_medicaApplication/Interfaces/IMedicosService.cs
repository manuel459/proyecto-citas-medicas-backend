using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace Consulta_medica.Application.Interfaces
{
    public interface IMedicosService
    {
        Task<Response> GetMedicos([FromRoute] string usuario, [FromQuery] RequestGenericFilter request);
        Task<Response> AddMedico(MedicoRequestDto request);
        Task<Response> UpdateMedico(MedicoRequestDto request);
        Task<Response> DeleteMedico(string id, int nEstado);
        Task<Response> sendBusinessHours(string Codmed);
    }
}
