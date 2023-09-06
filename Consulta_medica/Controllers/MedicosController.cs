using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Sevurity;
using Consulta_medica.Sevurity.Enum;
using Consulta_medica.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicosService _medicosService;
        public MedicosController(IMedicosService medicosService)
        {
            _medicosService = medicosService;
        }

        [HttpGet("{usuario}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.LIST_MODULE_MEDICOS, Permissions.LIST_MODULE_MEDICOS_INDIVIDUAL)]
        public async Task<IActionResult> GetMedicos([FromRoute] string usuario, [FromQuery] RequestGenericFilter request)
        {
            var orespuesta = await _medicosService.GetMedicos(usuario, request);
            return Ok(orespuesta);  
        }

        [HttpPost]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_CREATE_MEDICO)]
        public async Task<IActionResult> AddMedico(MedicoRequestDto request) 
        {
            var orespuesta = await _medicosService.AddMedico(request);
            return Ok(orespuesta);
        }

        [HttpPut]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_EDIT_MEDICO)]
        public async Task<IActionResult> UpdateMedico(MedicoRequestDto request)
        {
            var orespuesta = await _medicosService.UpdateMedico(request);
            return Ok(orespuesta);
        }

        [HttpDelete("{id}/{nEstado}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_DELETE_MEDICO)]
        public async Task<IActionResult> Delete(string id, int nEstado) 
        {
            var orespuesta = await _medicosService.DeleteMedico(id, nEstado);
 
            return Ok(orespuesta);
        }

        [HttpGet("BusinessHours")]
        public async Task<IActionResult> sendBusinessHours(string Codmed) 
        {
            var orespuesta = await _medicosService.sendBusinessHours(Codmed);

            return Ok(orespuesta);
        }
    }
}
