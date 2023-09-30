using Consulta_medica.Dto.Request;
using Microsoft.AspNetCore.Http;

namespace Consulta_medica.Interfaces
{
    public interface IDiagnosticoRepository
    {
        public Task<bool> getUpdDiagnostico(DiagnosticoRequestPdfDto request, List<IFormFile> files);
        int getIdHistoricMedik(int nId_Cita);
    }
}
