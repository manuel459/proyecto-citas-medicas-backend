using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Consulta_medica.Sevurity.Enum;
using Consulta_medica.Sevurity;
using Consulta_medica.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiagnosticoController : ControllerBase
    {
        private readonly IDiagnosticoRepository _diagnostico;
        public DiagnosticoController(IDiagnosticoRepository diagnostico) 
        {
            _diagnostico = diagnostico;
        }

        [HttpPost("SaveHistoryMedic")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.GENERAR_DIAGNOSTICO)]
        public async Task<IActionResult> getUpdDiagnostico(DiagnosticoRequestPdfDto request)
        {
            Response orespuesta = new Response();
            try
            {
                var response = await _diagnostico.getUpdDiagnostico(request);
                if (response)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Paciente atendido!";
                    orespuesta.data = response;
                }
                else 
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de generar el diagnostico";
                    orespuesta.data = response;
                }
              
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }


    }
}
