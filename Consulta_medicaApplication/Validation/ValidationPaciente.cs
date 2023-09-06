using Consulta_medica.Dto.Request;
using Consulta_medica.Models;
using FluentValidation;

namespace Consulta_medica.Validation
{
    public class ValidationPaciente : AbstractValidator<PacienteRequestDto>
    {
  
        public ValidationPaciente()
        {
            RuleFor(x => x.Nomp).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo Nombre del paciente es requerido")
                .Matches("^[A-Z][a-z]").WithMessage("Por favor ingresa solo caracteres de tipo letra, como mínimo una letra mayúscula.")
                .NotNull();

            RuleFor(x => x.Dnip).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo DNI del paciente es requerido.")
                .Must(x => x <= 99999999 && x >= 10000000).WithMessage("El campo DNI debe contener 8 caracteres de tipo númerico.")
                            .Must((Paciente,dni) =>
                            {
                                using (consulta_medicaContext db = new consulta_medicaContext())
                                {
                                    var lista = db.Paciente.Where(x => x.Dnip == dni && x.Id != Paciente.Id).Select(x => x.Dnip);
                                    return (!lista.Contains(dni));
                                }
                            }).WithMessage("Este paciente ya existe en la base de datos"); 

            RuleFor(customer => customer.correoElectronico).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Email es requerido.")
                .EmailAddress()
                    .WithMessage("Por favor ingresa un email valido.");
            


            RuleFor(x => x.Numero).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo Número de Teléfono es requerido.")
                .Must(x => x <= 999999999 && x >= 100000000).WithMessage("Por favor ingrese un Número de Teléfono válido.");
        }
    }
}
