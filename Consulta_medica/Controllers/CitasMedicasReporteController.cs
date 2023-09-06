using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitasMedicasReporteController : ControllerBase
    {
        private readonly ICitasMedicasReporteRepository _citasMedicasReporteRepository;

        public CitasMedicasReporteController(ICitasMedicasReporteRepository citasMedicasReporteRepository) 
        {
            _citasMedicasReporteRepository = citasMedicasReporteRepository;
        }
        [HttpGet]
        public async Task<IActionResult> getReporte(RequestFilterDtoNew request) 
        {
            Response response = new();
            try
            {
                var listReporte = await _citasMedicasReporteRepository.getReporte(request);
                if (listReporte.Count().Equals(0))
                {
                    response.exito = 0;
                    response.mensaje = "no se encontraron registros";
                    response.data = listReporte;
                }
                response.exito = 1;
                response.mensaje = "reporte traido con exito";
                response.data = listReporte;
            }
            catch (Exception ex)
            {
                response.mensaje = ex.Message;
            }
          
            return Ok(response);
        }
    }
}
