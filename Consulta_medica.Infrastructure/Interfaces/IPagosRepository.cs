using Consulta_medica.Dto.Response;
using Consulta_medica.Models;
using System.Threading.Tasks;

namespace Consulta_medica.Interfaces
{
    public interface IPagosRepository
    {
        Task<DetailPagoResponse> getInfoPago(int sId_Cita);
        Task<bool> InsertPagoCita(Pagos request);
        Task<bool> UpdateEstadoPagoCita(int nId_Cita);
        string getDocumentPagos(Pagos pagos);
    }
}
