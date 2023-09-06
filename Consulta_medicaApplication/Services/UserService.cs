using Consulta_medica.Application.Interfaces;
using Consulta_medica.Application.Validation;
using Consulta_medica.Common;
using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Infrastructure.Repositories;
using Consulta_medica.Models;
using Consulta_medica.Validation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Consulta_medica.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly consulta_medicaContext _context;
        private readonly ValidateRestorePassword _validationRestorePassword;

        public UserService(IOptions<AppSettings> appSettings, consulta_medicaContext context, IUnitOfWork unitOfWork, ValidateRestorePassword validationRestorePassword)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _unitOfWork = unitOfWork;
            _validationRestorePassword = validationRestorePassword;
        }

        public UserResponseDto Auth(LogeoRequestDto model)
        {
            UserResponseDto userResponse = new UserResponseDto();
            string contraseña = model.Contraseña;
            bool passwordMatches = false;

            var medico = _context.Medico.Where(d => d.Correo == model.CorreoElectronico).FirstOrDefault();
            var user = _context.Usuarios.Where(d => d.sCorreo == model.CorreoElectronico).FirstOrDefault();    

            if (medico != null)
            {
                passwordMatches = BCrypt.Net.BCrypt.Verify(contraseña, medico.Pswd);
                if (passwordMatches)
                {
                    userResponse.CorreoElectronico = medico.Correo;
                    userResponse.Token = GetToken<Medico>(medico.Codmed.ToString(), medico.Correo.ToString(), medico.Idtip);
                    userResponse.Nombre = medico.Nombre + " " + medico.sApellidos;
                    userResponse.Idtip = medico.Idtip;
                }

            }
            else if (user != null)
            {
                passwordMatches = BCrypt.Net.BCrypt.Verify(contraseña, user.sPswd);
                if (passwordMatches)
                {
                    userResponse.CorreoElectronico = user.sCorreo;
                    userResponse.Token = GetToken<Usuarios>(user.nIdUser.ToString(), user.sCorreo, user.nIptip);
                    userResponse.Nombre = user.sNombres + " " + user.sApellidos;
                    userResponse.Idtip = user.nIptip;
                }

            }

            return userResponse;
        }

        public string GetToken<T>(string codigo, string email, string Idtip)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier,codigo.ToString()),
                         new Claim(ClaimTypes.Email, email),
                         new Claim(ClaimTypes.Actor,Idtip)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Response> GetUser(RequestGenericFilter request, string usuario)
        {
            Response orespuesta = new Response();
            try
            {
                var response = await _unitOfWork.User.getUser(request, usuario);
                orespuesta.exito = 1;
                orespuesta.mensaje = "Listado traido con exito";
                orespuesta.data = response;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }
        public async Task<Response> AddUser(Usuarios request)
        {
            Response orespuesta = new Response();
            try
            {
                //HASHEAR LA CONTRASEÑA
                request.sPswd = BCrypt.Net.BCrypt.HashPassword(request.sPswd);

                var result = await _unitOfWork.User.AddUser(request);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Usuario insertado con exito";
                    orespuesta.data = result;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de registrar al usuario";
                    orespuesta.data = result;
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }
        public async Task<Response> UpdateUser(Usuarios request)
        {
            Response orespuesta = new Response();
            try
            {
                //HASHEAR LA CONTRASEÑA ACTUALIZADA
                request.sPswd = string.IsNullOrEmpty(request.sPswd) ? request.sPswd : BCrypt.Net.BCrypt.HashPassword(request.sPswd);

                var result = await _unitOfWork.User.UpdateUser(request);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Usuario editado con exito";
                    orespuesta.data = result;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de actualizar al Usuario";
                    orespuesta.data = result;
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> DeleteUser(int id)
        {
            Response orespuesta = new Response();
            try
            {
                var result = await _unitOfWork.User.DeleteUser(id);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Usuario eliminado con exito";
                    orespuesta.data = result;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un error eliminar el registro";
                    orespuesta.data = result;
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> restorePassword(restorePasswordRequestDto request)
        {
            bool recordAffected;
            Response orespuesta = new Response();
            try
            {
                var validation = await _validationRestorePassword.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    orespuesta.exito = 0;
                    orespuesta.mensaje = "Errores de validación";
                    orespuesta.Errors = validation.Errors;
                    return orespuesta;
                }

                //OBTIENE LOS PASSWORD HASHEADOS SEGUN EL EMAIL
                var result = await _unitOfWork.User.getEmail(request.sEmail);

                //VALIDA QUE LA CONTRASEÑA ANTERIOR COINCIDA CON LA ENVIADA EN OLDPASSWORD
                bool passwordMatches = BCrypt.Net.BCrypt.Verify(request.sOldPassword, result.sPswd);

                if (passwordMatches)
                {
                    //HASHEAR LA CONTRASEÑA ACTUALIZADA
                    request.sNewPassword = string.IsNullOrEmpty(request.sNewPassword) ? request.sNewPassword : BCrypt.Net.BCrypt.HashPassword(request.sNewPassword);

                    //SETEAR LA NUEVA CONTRASEÑA
                    result.sPswd = request.sNewPassword;

                    switch (result.sRol)
                    {
                        case "U002": 
                                recordAffected = await _unitOfWork.User.restorePasswordMedico(result);
                            break;
                        default:
                                recordAffected = await _unitOfWork.User.restorePasswordUsuario(result);
                            break;
                    }
                    if (recordAffected)
                    {
                        orespuesta.exito = 1;
                        orespuesta.mensaje = "Contraseña restablecida con exito";
                        orespuesta.data = result;
                    }
                    else
                    {
                        orespuesta.mensaje = "Ocurrio un error al momento de restablecer";
                        orespuesta.data = result;
                    }
                }
                else 
                {
                    orespuesta.mensaje = "Por favor valide que la contraseña anterior sea la correcta.";
                }

            }
            catch (Exception ex)
            {

                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }
    }
}
