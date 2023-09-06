using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Validation;

namespace Consulta_medica.Application.Services
{
    public class PacienteService : IPacienteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationPaciente _validationPaciente;
        public PacienteService(IUnitOfWork unitOfWork, ValidationPaciente validationPaciente) 
        {
            _unitOfWork = unitOfWork;
            _validationPaciente = validationPaciente;
        }
        public async Task<Response> GetPacientes(RequestGenericFilter request, string usuario)
        {
            Response orespuesta = new Response();
            try
            {
                var response = await _unitOfWork.Pacientes.GetPacientes(request,usuario);
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
        public async Task<Response> AddPaciente(PacienteRequestDto request)
        {
            Response orespuesta = new Response();
            try
            {
                var validation = await _validationPaciente.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    orespuesta.exito = 0;
                    orespuesta.mensaje = "Errores de validación";
                    orespuesta.Errors = validation.Errors;
                    return orespuesta;
                }

                var result = await _unitOfWork.Pacientes.AddPaciente(request);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Paciente insertado con exito";
                    orespuesta.data = result;
                }
                else 
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de registrar al paciente";
                    orespuesta.data = result;
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }
        public async Task<Response> UpdatePaciente(PacienteRequestDto request)
        {
            Response orespuesta = new Response();
            try
            {
                var validation = await _validationPaciente.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    orespuesta.exito = 0;
                    orespuesta.mensaje = "Errores de validación";
                    orespuesta.Errors = validation.Errors;
                    return orespuesta;
                }

                var result = await _unitOfWork.Pacientes.UpdatePaciente(request);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Paciente editado con exito";
                    orespuesta.data = result;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de actualizar al paciente";
                    orespuesta.data = result;
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> DeletePaciente(int id)
        {
            Response orespuesta = new Response();
            try
            {
                var result = await _unitOfWork.Pacientes.DeletePaciente(id);
                if (result)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Paciente eliminado con exito";
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
    }
}
