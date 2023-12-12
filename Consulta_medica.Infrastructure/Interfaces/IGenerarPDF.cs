using Consulta_medica.Dto.Request;
using Microsoft.AspNetCore.Http;

namespace Consulta_medica.Interfaces
{
    public interface IGenerarPDF
    {
        string GenerateInvestorDocument(CitasRequestDto contractInfo);
        string MessageTemplate(int codigo);
        void EnvioNotificationGeneric(string CorreoDestino, string Encabezado, string bodyMessage, string? newDocumentFileName);
        (string,string) getDocumentPagos(PagosRequestDto pagos);
        bool FileGenericGenerate(string sEntidad, int nId_Entidad, List<IFormFile> files);
        string getObjectS3(string objectKey);
    }
}
