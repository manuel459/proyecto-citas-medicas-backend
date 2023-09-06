using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace Consulta_medica.Application.Interfaces
{
    public interface ICitasMedicasService
    {
        Task<Response> GetCitas(RequestGenericFilter request, string usuario);
        Task<Response> AddCitas(CitasRequestDto request);
        Task<Response> UpdateCitas(CitasRequestDto request);
        Task<Response> DeleteCitas(int id);
        Task<Response> ConsultarDni(CitasRequestDniDto Personal);
        Task<Response> ConsultarHorario(DatosRequestCitasDto request);
        Task<Response> obtenerHistoriaMedica([FromRoute] int dnip);
        Task<bool> RecordatorioNotification();
    }
}
