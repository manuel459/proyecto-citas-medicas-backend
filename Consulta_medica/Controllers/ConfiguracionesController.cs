using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfiguracionesController : ControllerBase
    {
        private IConfiguracionesRepository _configuraciones;
        public ConfiguracionesController(IConfiguracionesRepository configuraciones) 
        {
            _configuraciones = configuraciones;
        }
        [HttpGet]
        public async Task<IActionResult> getConfiguraciones([FromQuery] string sEntidad, [FromQuery] string sId) 
        {
            Response orespuesta = new();
            try
            {
                var configuraciones = await _configuraciones.getConfiguraciones(sEntidad, sId);
                if (configuraciones.Count().Equals(0))
                {
                    orespuesta.mensaje = "No se encontraron registros";
                    orespuesta.exito = 0;
                    orespuesta.data = configuraciones;
                    return Ok(orespuesta);
                }
                orespuesta.data = configuraciones;
                orespuesta.mensaje = "consulta exitosa";
                orespuesta.exito = 1;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }

        [HttpGet("Grafica_Citas")]
        public async Task<IActionResult> getGraficaCitas() 
        {
            Response orespuesta = new();
            try
            {
                var grafica = await _configuraciones.getGraficaCitas();
                if (grafica.Count().Equals(0))
                {
                    orespuesta.mensaje = "No se encontraron registros";
                    orespuesta.exito = 0;
                    orespuesta.data = grafica;
                    return Ok(orespuesta);
                }
                orespuesta.data = grafica;
                orespuesta.mensaje = "consulta exitosa";
                orespuesta.exito = 1;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }

        [HttpGet("Grafica_Citas_Disponibles")]
        public async Task<IActionResult> getGraficaCitasDisponibles([FromQuery] string codmed, [FromQuery] string dFecha_Consulta)
        {
            Response orespuesta = new();
            try
            {
                var grafica = await _configuraciones.getGraficaCitasDisponibles(codmed, dFecha_Consulta);
                if (grafica == null)
                {
                    orespuesta.mensaje = "No se encontraron registros";
                    orespuesta.exito = 0;
                    orespuesta.data = grafica;
                    return Ok(orespuesta);
                }
                orespuesta.data = grafica;
                orespuesta.mensaje = "consulta exitosa";
                orespuesta.exito = 1;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }
    }
}
