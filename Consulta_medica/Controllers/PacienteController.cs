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
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteServices _pacienteServices;
        public PacienteController(IPacienteServices pacienteServices)
        {
            _pacienteServices = pacienteServices;
        }

        [HttpGet("{usuario}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.LIST_MODULE_PACIENTES, Permissions.LIST_MODULE_PACIENTES_INDIVIDUAL)]
        public async Task<IActionResult> GetPacientes([FromRoute] string usuario, [FromQuery] RequestGenericFilter request)
        {
            var response = await _pacienteServices.GetPacientes(request, usuario);
            return Ok(response);
        }

        [HttpPost]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_CREATE_PACIENTE)]
        public async Task<IActionResult> AddPaciente(PacienteRequestDto request)
        {
            var response = await _pacienteServices.AddPaciente(request);
            return Ok(response);
        }

        [HttpPut]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_EDIT_PACIENTE)]
        public async Task<IActionResult> UpdatePaciente(PacienteRequestDto request)
        {
            var response = await _pacienteServices.UpdatePaciente(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_DELETE_PACIENTE)]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var response = await _pacienteServices.DeletePaciente(id);
            return Ok(response);
        }

    }
}
