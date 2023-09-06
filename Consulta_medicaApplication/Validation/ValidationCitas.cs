using Consulta_medica.Dto.Request;
using FluentValidation;

namespace Consulta_medica.Validation
{
    public class ValidationCitas : AbstractValidator<CitasRequestDto>
    {
        public ValidationCitas() 
        {
            RuleFor(x =>x.Dnip)
                .NotEmpty().WithMessage("Por favor ingresa el numero de Dni")
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Dnip)
                    .Must(x => x <= 99999999 && x >= 10000000).WithMessage("El campo Dni debe contener 8 caracteres");
                });

            RuleFor(x => x.Codes)
                .NotEmpty().WithMessage("Por favor indica una especialidad")
                .NotNull();

            RuleFor(x => x.Codmed)
                .NotEmpty().WithMessage("Por favor selecciona a un medico")
                .NotNull();

            RuleFor(x => x.Hora)
                .NotEmpty().WithMessage("Por favor selecciona horario")
                .NotNull();

            RuleFor(x => x.Feccit)
              .NotEmpty().WithMessage("Por favor selecciona una fecha")
              .NotNull();

        }
    }
}
