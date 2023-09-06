using Consulta_medica.Dto.Request;
using FluentValidation;

namespace Consulta_medica.Validation
{
    public class ValidateDiagnosticoRequest : AbstractValidator<DiagnosticoRequestPdfDto>
    {
        public ValidateDiagnosticoRequest() 
        {
            RuleFor(x => x.diagnostico)
                .NotEmpty().WithMessage("Por favor ingresa un Diagnostico")
                .NotNull();
            RuleFor(x => x.medicamentos)
                .NotEmpty().WithMessage("Por favor ingresa la Receta Medico")
                .NotNull();
        }
    }
}
