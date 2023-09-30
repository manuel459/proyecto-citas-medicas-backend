using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace Consulta_medica.Application.Interfaces
{
    public interface IDiagnosticoService
    {
        Task<Response> getUpdDiagnostico(DiagnosticoRequestPdfDto request, List<IFormFile> files);
    }
}
