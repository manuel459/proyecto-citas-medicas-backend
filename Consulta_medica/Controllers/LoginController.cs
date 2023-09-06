using Consulta_medica.Application.Interfaces;
using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Consulta_medica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Autenticar([FromBody] LogeoRequestDto model)
        {
            Response respuesta = new Response();
            var userresponse = _userService.Auth(model);
            if (userresponse.Token == null)
            {
                respuesta.exito = 0;
                respuesta.mensaje = "Usuario o contraseña incorrecta";
                return BadRequest(respuesta);
            }

            respuesta.exito = 1;
            respuesta.mensaje = "Ingreso correcto";
            respuesta.data = userresponse;
            return Ok(respuesta);
        }

        [HttpPost("restorePassword")]
        public async Task<IActionResult> restorePassword(restorePasswordRequestDto request) 
        {
            var result = await _userService.restorePassword(request);
            return Ok(result);
        }


    }
}
