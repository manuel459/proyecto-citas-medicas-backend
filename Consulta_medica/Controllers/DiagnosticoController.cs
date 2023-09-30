using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Sevurity;
using Consulta_medica.Sevurity.Enum;
using Consulta_medica.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiagnosticoController : ControllerBase
    {
        private readonly IDiagnosticoService _diagnostico;
        public DiagnosticoController(IDiagnosticoService diagnostico) 
        {
            _diagnostico = diagnostico;
        }

        [HttpPost("SaveHistoryMedic")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.GENERAR_DIAGNOSTICO)]
        public async Task<IActionResult> getUpdDiagnostico([FromForm] DiagnosticoRequestPdfDto request, List<IFormFile> files)
        {
            var orespuesta = await _diagnostico.getUpdDiagnostico(request, files);
            
            return Ok(orespuesta);
        }


    }
}
