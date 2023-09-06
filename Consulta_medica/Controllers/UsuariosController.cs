using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Models;
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
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsuariosController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet("{usuario}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.LIST_MODULE_USER, Permissions.LIST_MODULE_USER_INDIVIDUAL)]
        public async Task<IActionResult> GetUser([FromRoute] string usuario, [FromQuery] RequestGenericFilter request)
        {
            var response = await _userService.GetUser(request, usuario);
            return Ok(response);
        }

        [HttpPost]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_CREATE_USUARIOS)]
        public async Task<IActionResult> AddUser(Usuarios request)
        {
            var response = await _userService.AddUser(request);
            return Ok(response);
        }

        [HttpPut]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_EDIT_USUARIOS)]
        public async Task<IActionResult> UpdateUser(Usuarios request)
        {
            var response = await _userService.UpdateUser(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [PermisosAtributte(PermissionOperator.Or, Permissions.BUTTON_DELETE_USUARIOS)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            return Ok(response);
        }
    }
}
