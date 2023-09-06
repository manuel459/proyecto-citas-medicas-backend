using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPagosRepository _pagosRepository;
        private readonly IGenerarPDF _generarPDF;

        public PagosController(IPagosRepository pagosRepository, IGenerarPDF generarPDF) 
        {
            _pagosRepository = pagosRepository;
            _generarPDF = generarPDF;
        }

        [HttpGet("{sId_Cita}")]
        public async Task<IActionResult> getInfoPagos(int sId_Cita) 
        {
            Response response = new();
            try
            {
                var cita = await _pagosRepository.getInfoPago(sId_Cita);

                if (cita is null)
                {
                    response.exito = 0;
                    response.mensaje = "No se encontraton registros";
                }
                else
                {
                    response.exito = 1;
                    response.mensaje = "Consulta exitosa";
                    response.data = cita;
                }
            }
            catch (System.Exception ex)
            {
                response.mensaje = ex.Message;
                response.exito = 0;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> getInfoPagos(Pagos request)
        {
            Response response = new();
            try
            {
                var cita = await _pagosRepository.InsertPagoCita(request);

                if (!cita)
                {
                    response.exito = 0;
                    response.mensaje = "Ocurrio un problema al momento de insertar el registro";
                    return Ok(response);
                }

                var updateCita = await _pagosRepository.UpdateEstadoPagoCita(request.sCod_Cita);
                if (!updateCita)
                {
                    response.exito = 0;
                    response.mensaje = "Ocurrio un problema al momento de actualizar el registro";
                    return Ok(response);
                }

                var boletaPago = _pagosRepository.getDocumentPagos(request);

                response.exito = 1;
                response.mensaje = "Registro exitoso";
                response.data = boletaPago;

            }
            catch (System.Exception ex)
            {
                response.mensaje = ex.Message;
                response.exito = 0;
            }

            return Ok(response);
        }
    }
}
