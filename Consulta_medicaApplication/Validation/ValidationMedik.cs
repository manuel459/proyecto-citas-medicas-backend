using Consulta_medica.Dto.Request;
using Consulta_medica.Models;
using FluentValidation;

namespace Consulta_medica.Validation
{
    public class ValidationMedik : AbstractValidator<MedicoRequestDto>
    {
        public ValidationMedik() 
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("Por favor ingresa el primer nombre")
                .NotNull();

            RuleFor(x => x.Dni)
                .NotEmpty().WithMessage("Por favor ingresa el numero de Dni")
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Dni)
                    .Must(x => x <= 99999999 && x >= 10000000).WithMessage("El campo Dni debe contener 8 caracteres");
                }).Must((Medico,id) =>
                {
                    using (consulta_medicaContext db = new consulta_medicaContext())
                    {
                        var lista = db.Medico.Where(x => x.Dni == id && x.Codmed != Medico.Codmed).Select(x => x.Dni);
                        return (!lista.Contains(id));
                    }
                }).WithMessage("Dni existente");

            RuleFor(x => x.Correo)
               .Must((Medico, id) =>
               {
                   using (consulta_medicaContext db = new consulta_medicaContext())
                   {
                       var lista = db.Medico.Where(x => x.Correo == Medico.Correo && x.Codmed != Medico.Codmed).Select(x => x.Correo);
                       return (!lista.Contains(Medico.Correo));
                   }
               }).WithMessage("Correo existente");

            RuleFor(x => x.Sexo)
                .NotEmpty().WithMessage("Por favor indica tu sexo")
                .NotNull();

            RuleFor(x => x.Nac)
                .NotEmpty().WithMessage("Por favor indica tu Fecha de Nacimiento")
                .NotNull();

            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("Por favor ingresa tu correo")
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Correo)
                    .EmailAddress()
                    .WithMessage("Por favor ingresa un email válido");
                });


            //RuleFor(x => x.Pswd)
            //    .NotEmpty().WithMessage("Por favor ingresa tu contraseña")
            //    .NotNull()
            //    .DependentRules(() =>
            //    {
            //        RuleFor(x => x.Pswd)
            //           .Matches("[A-Z]").WithMessage("La contraseña debe tener al menos un cáracter en mayuscula")
            //           .Matches("[a-z]").WithMessage("La contraseña debe tener al menos un cáracter en miniscula")
            //           .Matches("[0-9]").WithMessage("La contraseña debe tener al menos un número")
            //           .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
            //           .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe tener al menos un caracter especial");
            //    });
                

            RuleFor(x => x.Idhor)
             .NotEmpty().WithMessage("Por favor ingresa tu Horario")
             .NotNull();

            RuleFor(x => x.Codes)
            .NotEmpty().WithMessage("Por favor indica la especialidad")
            .NotNull();
        }
    }
}
