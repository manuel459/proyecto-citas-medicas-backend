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
    public class CitasMedicasController : ControllerBase
    {
        private readonly ICitasMedicasService _citasService;
        public CitasMedicasController(ICitasMedicasService citasService) 
        {
            _citasService = citasService;
        }

        [HttpGet("{usuario}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.LIST_MODULE_NEWCITA_PACIENTES, Permissions.LIST_MODULE_NEWCITA)]
        public async Task<IActionResult> GetCitas([FromRoute] string usuario, [FromQuery] RequestGenericFilter request)
        {
            var response = await _citasService.GetCitas(request, usuario);
           
            return Ok(response);
        }

        [HttpPost]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_CREATE_CITA)]
        public async Task<IActionResult> AddCitas(CitasRequestDto request) 
        {
            var response = await _citasService.AddCitas(request);
            return Ok(response);
        }

        [HttpPut]
        [PermisosAtributte(PermissionOperator.Or, Permissions.VIEW_EDIT_CITA)]
        public async Task<IActionResult> UpdateCitas(CitasRequestDto request)
        {
            var lst = await _citasService.UpdateCitas(request);
               
            return Ok(lst);
        }

        [HttpDelete("{id}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.VIEW_DELETE_CITA)]
        public async Task<IActionResult> DeleteCitas(int id)
        {
            var response = await _citasService.DeleteCitas(id);
            
            return Ok(response);
        }

        [HttpPost("DniPaciente")]
        public async Task<IActionResult> ConsultarDni(CitasRequestDniDto Personal)
        {
            var response = await _citasService.ConsultarDni(Personal);
           
            return Ok(response);
        }

        [HttpPost("Horario")]
        public async Task<IActionResult> ConsultarHorario(DatosRequestCitasDto request) 
        {
            var response = await _citasService.ConsultarHorario(request);
            return Ok(response);
        }

        [HttpGet("HistoricMedik/{dnip}")]
        public async Task<IActionResult> obtenerHistoriaMedica([FromRoute] int dnip)
        {
            var response = await _citasService.obtenerHistoriaMedica(dnip);
            return Ok(response);
        }
    }
}
