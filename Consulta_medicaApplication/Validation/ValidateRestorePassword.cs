using Consulta_medica.Domain.DTOs.Request;
using Dapper;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Consulta_medica.Application.Validation
{
    public class ValidateRestorePassword : AbstractValidator<restorePasswordRequestDto>
    {
        private readonly IConfiguration _configuration;
        public ValidateRestorePassword(IConfiguration configuration) 
        {
            _configuration = configuration;

            RuleFor(x => x.sNewPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo sNewPassword del paciente es requerido.")
                            .Must((Usuario, sNewPassword) =>
                            {
                                using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

                                connection.Open();

                                string sqlQuery = "select sPswd from (\r\nselect sCorreo sEmail, sPswd from Usuarios u\r\nunion all\r\nselect correo, m.pswd from medico m\r\n)a where sEmail = @sEmail;";

                                // Dapper query and object mapping
                                var sOldPswd = connection.QueryFirstOrDefault<string>(sqlQuery, new { sEmail = Usuario.sEmail });

                                //VALIDA QUE LA NUEVA CONTRASEÑA NO SEA IGUAL A LA QUE YA EXISTE
                                bool passwordMatches = BCrypt.Net.BCrypt.Verify(Usuario.sNewPassword, sOldPswd);

                                return !passwordMatches;

                            }).WithMessage("La nueva contraseña no puede ser igual a la anterior");
        }
    }
}
