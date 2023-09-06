using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Validation;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Consulta_medica.Application.Services
{
    public class MedicosService : IMedicosService
    {
        private readonly ValidationMedik _validationMedik;
        private readonly IUnitOfWork _unitOfWork;
        public MedicosService(IUnitOfWork unitOfWork, ValidationMedik validationMedik)
        {
            _unitOfWork = unitOfWork;
            _validationMedik = validationMedik;
        }

        public async Task<Response> GetMedicos(string usuario, RequestGenericFilter request)
        {
            Response orespuesta = new Response();
            try
            {
                var lst = await _unitOfWork.Medicos.GetMedicos(request, usuario);
                orespuesta.exito = 1;
                orespuesta.mensaje = "Medico traido con exito";
                orespuesta.data = lst;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> AddMedico(MedicoRequestDto request)
        {
            Response orespuesta = new Response();
            try
            {
                var validation = await _validationMedik.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    orespuesta.exito = 0;
                    orespuesta.mensaje = "Errores de validación";
                    orespuesta.Errors = validation.Errors;
                    return orespuesta;
                }

                //HASHEAR LA CONTRASEÑA
                request.Pswd = BCrypt.Net.BCrypt.HashPassword(request.Pswd);

                //INSERTAR AL MEDICO
                var insert = await _unitOfWork.Medicos.AddMedico(request);

                if (insert.Item1)
                {

                    //GENERAR EL BODY DE SU HORARIO
                    string bodyMessage = await createBodyHorario(insert.Item2);

                    //ACTUALIZAR BODY EN LA COLUMNA
                    var recordAffected = await _unitOfWork.Medicos.UpdateBodyHorario(insert.Item2, bodyMessage);

                    //OPCIONAL
                    if (request.bActiveNotificaciones)
                    {
                        //ENVIAR NOTIFICACIÓN DE HORARIO
                        _unitOfWork.generarPDF.EnvioNotificationGeneric(request.Correo, "HORARIO MÉDICO", bodyMessage, null);
                    }
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Médico insertado con exito";
                    orespuesta.data = insert;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un Error al momento de registrar al médico";
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> UpdateMedico(MedicoRequestDto request)
        {
            Response orespuesta = new Response();
            try
            {
                var validation = await _validationMedik.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    orespuesta.exito = 0;
                    orespuesta.mensaje = "Errores de validación";
                    orespuesta.Errors = validation.Errors;
                    return orespuesta;
                }

                //HASHEAR LA CONTRASEÑA ACTUALIZADA
                request.Pswd = string.IsNullOrEmpty(request.Pswd)?request.Pswd : BCrypt.Net.BCrypt.HashPassword(request.Pswd);


                var response = await _unitOfWork.Medicos.UpdateMedico(request);
                if (response)
                {

                    //GENERAR EL BODY DE SU HORARIO
                    string bodyMessage = await createBodyHorario(request.Codmed);

                    //ACTUALIZAR BODY EN LA COLUMNA
                    var recordAffected = await _unitOfWork.Medicos.UpdateBodyHorario(request.Codmed, bodyMessage);

                    //OPCIONAL
                    if (request.bActiveNotificaciones)
                    {
                        //ENVIAR NOTIFICACIÓN DE HORARIO
                        _unitOfWork.generarPDF.EnvioNotificationGeneric(request.Correo, "HORARIO MÉDICO", bodyMessage, null);
                    }
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Médico editado con exito";
                    orespuesta.data = response;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de editar los datos del médico o no se detectaron cambios en el formulario";
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<string> createBodyHorario(string Codmed) 
        {
            var horarioObject = await _unitOfWork.Medicos.ObtenerInfoHorario(Codmed);

            string bodyMessage = $"Estimado {horarioObject.FirstOrDefault().sNombres}, adjunto su jornada laboral asignada.";

            bodyMessage += "<html>\r\n" +
            "          <head>\r\n  " +
            "           <title>Horario del trabajador</title>\r\n  " +
            "           <style>\r\n    " +
            "               table {\r\n border-collapse: collapse;\r\n    }\r\n    th, td {\r\n      border: 1px solid black;\r\n      padding: 8px;\r\n    }\r\n  " +
            "           </style>\r\n" +
            "          </head>\r\n" +
            "          <body>\r\n  " +
            "           <table>\r\n    " +
            "            <thead>\r\n      " +
            "              <tr>\r\n        " +
            "               <th>Código de Médico</th>\r\n        " +
            "               <th>Nombre del Médico</th>\r\n        " +
            "               <th>Codigo de Horario</th>\r\n        " +
            "               <th>Día de la semana</th>\r\n        " +
            "               <th>Jornada</th>\r\n      " +
            "              </tr>\r\n    " +
            "            </thead>\r\n    ";

            bodyMessage += "<tbody>";

            foreach (var item in horarioObject)
            {
                bodyMessage += $"<tr><td>{item.codmed}</td><td>{item.sNombres}</td><td>{item.idhor}</td><td>{item.sDias_semana}</td><td>{item.JornadaLaboral}</td></tr>";
            }

            bodyMessage +=
            "           </table>\r\n" +
            "          </body>\r\n" +
            "         </html>\r\n";

            return bodyMessage;
        }

       

        public async Task<Response> DeleteMedico(string id, int nEstado)
        {
            Response orespuesta = new Response();
            try
            {
                var lst = await _unitOfWork.Medicos.DeleteMedico(id, nEstado);
                if (lst)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Estado del médico actualizado con exito";
                    orespuesta.data = lst;
                }
                else
                {
                    orespuesta.mensaje = "Ocurrio un Error al momento de actualizar el estado del médico";
                }
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }
            return orespuesta;
        }

        public async Task<Response> sendBusinessHours(string Codmed)
        {
            Response orespuesta = new Response();

            try
            {
                var response = await _unitOfWork.Medicos.BusinessHours(Codmed);

                if (response is not null)
                {
                    _unitOfWork.generarPDF.EnvioNotificationGeneric(response.Correo, "HORARIO MÉDICO", response.sBodyHorario, null);

                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Envío de Horario Exitoso";
                }
                else
                {
                    orespuesta.mensaje = "Hubo un error al momento de envíar el Horario";
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
