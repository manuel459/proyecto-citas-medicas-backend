using Consulta_medica.Dto.Request;

namespace Consulta_medica.Interfaces
{
    public interface IGenerarPDF
    {
        string GenerateInvestorDocument(CitasRequestDto contractInfo);
        string MessageTemplate(int codigo);
        void EnvioNotificationGeneric(string CorreoDestino, string Encabezado, string bodyMessage, string? newDocumentFileName);
        (string,string) getDocumentPagos(PagosRequestDto pagos);
    }
}
