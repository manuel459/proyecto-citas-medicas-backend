using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Enum;
using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Static;
using Consulta_medica.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Response = Consulta_medica.Dto.Response.Response;

namespace Consulta_medica.Application.Services
{
    public class CitasMedicasService : ICitasMedicasService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationCitas _validationCitas;
        public CitasMedicasService(IUnitOfWork unitOfWork, ValidationCitas validationCitas) 
        {
            _unitOfWork = unitOfWork;
            _validationCitas = validationCitas;
        }

        public async Task<Response> GetCitas(RequestGenericFilter request, string usuario)
        {
            Response orespuesta = new Response();
            try
            {
                var lst = await _unitOfWork.Citas.GetCitas(request, usuario);
                orespuesta.exito = 1;
                orespuesta.mensaje = "citas traida con exito";
                orespuesta.data = lst;
            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }

            return orespuesta;
        }

        public async Task<Response> AddCitas(CitasRequestDto request)
        {
            Response response = new();

            Citas ocitas = new Citas();
            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var validation = await _validationCitas.ValidateAsync(request);
                    if (!validation.IsValid)
                    {
                        response.exito = 0;
                        response.mensaje = "Errores de validación";
                        response.Errors = validation.Errors;
                        return response;
                    }

                    var result = await _unitOfWork.Citas.AddCitas(request);

                    if (result.Item2)
                    {
                        if (request.bActiveNotificaciones)
                        {
                            //Generar pdf de cita medica
                            request.Id = result.Item1.Id;
                            string newDocumentFileName = _unitOfWork.generarPDF.GenerateInvestorDocument(request);

                            //ENVIO DE NOTIFICACIÓN
                            _unitOfWork.generarPDF.EnvioNotificationGeneric(request.CorreoElectronico, "CITA MÉDICA", _unitOfWork.generarPDF.MessageTemplate((int)EnumMessage.MESSAGE_CITA).Replace("NOMBRE_PACIENTE", request.NombrePaciente), newDocumentFileName);

                            //ENVIO DE NOTIFICACIÓN A MEDICO
                            if (request.Feccit.Date == DateTime.Now.Date || (request.Feccit.Date == DateTime.Now.AddDays(1).Date && DateTime.Now.Hour > 6)) 
                            {
                                string bodyMessage = $"Estimado(a) {request.NombreMedico} tiene una nueva cita médica a su cargo.";

                                var resultado = mBodyMessage(bodyMessage, request);                

                                _unitOfWork.generarPDF.EnvioNotificationGeneric(resultado.Item2, "CITA PENDIENTE", resultado.Item1, null);
                            }

                        }
                        response.data = ocitas;
                        response.mensaje = "Cita creada exitosamente";
                        response.exito = 1;
                    }
                    else
                    {
                        response.mensaje.Equals("Ha ocurrido un error al momento de insertar la cita medica");
                    }


                    transaction.Complete();
                }
            }
            catch (TransactionAbortedException)
            {
                response.data = ocitas;
                response.mensaje = "Ha ocurrido un error al momento de generar la cita medica";
                response.exito = 0;
            }

            return response;
        }

        public (string, string) mBodyMessage(string bodyMessage, CitasRequestDto request) 
        {
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
                                "               <th>Nombre de la Especialidad</th>\r\n        " +
                                "               <th>Nombre del Paciente</th>\r\n        " +
                                "               <th>Hora de Cita</th>\r\n        " +
                                "              </tr>\r\n    " +
                                "            </thead>\r\n    ";

            bodyMessage += "<tbody>";

            bodyMessage += $"<tr><td>{request.Codmed}</td><td>{request.NombreMedico}</td><td>{request.NombreEspecialidad}</td><td>{request.NombrePaciente}</td><td>{request.Hora}</td></tr>";

            bodyMessage +=
            "           </table>\r\n" +
            "          </body>\r\n" +
            "         </html>\r\n";

            request.CorreoElectronicoMedico = _unitOfWork.Medicos.getEmailMedico(request.Codmed);

            return (bodyMessage, request.CorreoElectronicoMedico);
        }

        public async Task<Response> UpdateCitas(CitasRequestDto request)
        {
            Response response = new();

            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var validation = await _validationCitas.ValidateAsync(request);
                    if (!validation.IsValid)
                    {
                        response.exito = 0;
                        response.mensaje = "Errores de validación";
                        response.Errors = validation.Errors;
                        return response;
                    }

                    var result = await _unitOfWork.Citas.UpdateCitas(request);
                    if (result)
                    {
                        if (request.bActiveNotificaciones)
                        {
                            //Generar pdf de cita medica
                            string newDocumentFileName = _unitOfWork.generarPDF.GenerateInvestorDocument(request);

                            //ENVIO DE NOTIFICACIÓN
                            _unitOfWork.generarPDF.EnvioNotificationGeneric(request.CorreoElectronico, "CITA MÉDICA", _unitOfWork.generarPDF.MessageTemplate((int)EnumMessage.MESSAGE_CITA).Replace("NOMBRE_PACIENTE", request.NombrePaciente), newDocumentFileName);

                            //ENVIO DE NOTIFICACIÓN A MEDICO
                            if (request.Feccit.Date == DateTime.Now.Date || (request.Feccit.Date == DateTime.Now.AddDays(1).Date && DateTime.Now.Hour > 6))
                            {
                                string bodyMessage = $"Estimado(a) {request.NombreMedico}, se ha actualizado una cita médica a su cargo.";

                                var resultado = mBodyMessage(bodyMessage, request);

                                _unitOfWork.generarPDF.EnvioNotificationGeneric(resultado.Item2, "CITA PENDIENTE", resultado.Item1, null);
                            }
                        }
                        response.mensaje = "Cita Actualizada exitosamente";
                        response.exito = 1;
                    }
                    else
                    {
                        response.mensaje.Equals("Ha ocurrido un error momento de actualizar la cita medica");
                    }
                    transaction.Complete();
                }
               
            }
            catch (TransactionAbortedException)
            {
                response.mensaje = "Ha ocurrido un error al momento de actualizar la cita medica";
                response.exito = 0;
            }

            return response;
        }

        public async Task<Response> DeleteCitas(int id)
        {
            Response orespuesta = new Response();
            try
            {
                var response = await _unitOfWork.Citas.DeleteCitas(id);
                if (response)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "cita eliminada con exito";
                    orespuesta.data = response;
                }
                else 
                {
                    orespuesta.mensaje = "Ocurrio un error al momento de eliminar la cita";
                    orespuesta.data = response;
                }

            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }

            return orespuesta;
        }

        public async Task<Response> ConsultarDni(CitasRequestDniDto Personal)
        {

            Response orespuesta = new Response();
            try
            {
                var response = (await _unitOfWork.Pacientes.getPacientes()).FirstOrDefault(x => x.Dnip == Personal.Dnip);

                if (response is not null)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "dni traido con exito";
                    orespuesta.data = response;
                }
                else 
                {
                    orespuesta.mensaje = "No se encontraron resultados";
                    orespuesta.data = response;
                }

            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }

            return orespuesta;
        }

        public async Task<Response> ConsultarHorario(DatosRequestCitasDto request)
        {
            var horario = await _unitOfWork.Citas.consultarHorario(request);

            List<Content> Contenthoras = new List<Content>();

            for (int i = (int)TimeSpan.Parse(horario.Hinicio.ToString()).TotalHours; i < TimeSpan.Parse(horario.Hfin.ToString()).TotalHours; i++)
            {
                Content ocontent = new Content();

                ocontent.contentCitas = TimeSpan.Parse(i.ToString() + ":00" + ":00");
                var citasRegistradas = await _unitOfWork.Citas.citasRegistradas(request);

                ocontent.status = !citasRegistradas.Contains(ocontent.contentCitas);
                Contenthoras.Add(ocontent);
            }
            Response orespuesta = new Response();
            orespuesta.exito = 1;
            orespuesta.mensaje = "Horarios traidos con exito";
            orespuesta.data = Contenthoras;

            return orespuesta;
        }

        public async Task<Response> obtenerHistoriaMedica([FromRoute] int dnip)
        {
            Response orespuesta = new Response();
            try
            {
                var response = await _unitOfWork.Citas.obtenerHistoriaMedica(dnip);

                response.ForEach(x => x.lUrlBase.ForEach(x => x.sUrl = _unitOfWork.generarPDF.getObjectS3(x.sUrl)));

                if (response.Count() > 0)
                {
                    orespuesta.exito = 1;
                    orespuesta.mensaje = "Consulta exitosa";
                    orespuesta.data = response;
                }
                else 
                {
                    orespuesta.mensaje = "No se encontraron registros";
                    orespuesta.data = response;
                }

            }
            catch (Exception ex)
            {
                orespuesta.mensaje = ex.Message;
            }

            return orespuesta;
        }


        //FLUJO DE RECORDATORIO DE NOTIFICACIÓN
        public async Task<bool> RecordatorioNotification()
        {
            try
            {

                //Flag para envio de notificacion masiva
                var nCodigo = (await _unitOfWork.Configuraciones.getConfigById("Tb_Notification_Activa")).nCodigo;

                if (nCodigo.Equals(1))
                {
                    DateTime FechaMañana = DateTime.Now.AddDays(1).Date;

                    var requestList = await _unitOfWork.Citas.getCitasPending(FechaMañana);

                    foreach (var request in requestList)
                    {
                        string documento = _unitOfWork.generarPDF.GenerateInvestorDocument(request);

                        //ENVIO DE NOTIFIACIÓN

                        _unitOfWork.generarPDF.EnvioNotificationGeneric(request.CorreoElectronico, "CITA MÉDICA", _unitOfWork.generarPDF.MessageTemplate((int)EnumMessage.MESSAGE_RECORDATORIO_CITA).Replace("NOMBRE_PACIENTE", request.NombrePaciente).Replace("FECHA_CITA", request.Feccit.ToString("dd/MM/yyyy")).Replace("HORA_CITA", request.Hora), documento);
                    }


                    //ENVIO DE NOTIFICACIÓN A MÉDICO
                    var medicos_con_citas_pendientes = requestList.OrderBy(x => x.Hora).Select(x => new { x.Codmed, x.NombreMedico, x.CorreoElectronicoMedico }).Distinct().ToList();

                    foreach (var medico in medicos_con_citas_pendientes)
                    {
                        var citas_Pendientes = await _unitOfWork.Citas.getCitasPendingByCodmed(FechaMañana, medico.Codmed);


                        string bodyMessage = $"Estimado(a) {medico.NombreMedico}, tiene(s) {citas_Pendientes.Count().ToString()} cita(s) pendientes para mañana {FechaMañana.ToString("yyyy/MM/dd")}";

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
                        "               <th>Nombre de la Especialidad</th>\r\n        " +
                        "               <th>Nombre del Paciente</th>\r\n        " +
                        "               <th>Hora de Cita</th>\r\n        " +
                        "              </tr>\r\n    " +
                        "            </thead>\r\n    ";

                        bodyMessage += "<tbody>";

                        foreach (var paciente in citas_Pendientes)
                        {
                            bodyMessage += $"<tr><td>{medico.Codmed}</td><td>{medico.NombreMedico}</td><td>{paciente.Nombre}</td><td>{paciente.Nomp + " " + paciente.Apellidos}</td><td>{paciente.Hora.ToString("hh\\:mm")}</td></tr>";
                        }

                        bodyMessage +=
                        "           </table>\r\n" +
                        "          </body>\r\n" +
                        "         </html>\r\n";
                        //ENVIO DE NOTIFICACIÓN
                        _unitOfWork.generarPDF.EnvioNotificationGeneric(medico.CorreoElectronicoMedico, "CITAS PENDIENTES", bodyMessage, null);
                    }

                }
            }
            catch (Exception ex)
            {
                _unitOfWork.generarPDF.EnvioNotificationGeneric(GlobalMessageError.USER_ADMINISTRADOR, GlobalMessageError.ERROR_JOB_CABECERA, GlobalMessageError.ERROR_JOB + JobName.NOTI_RECORDATORIO_CITA +" :"+ ex.Message, null);
                return false;
            }

            return true;
        }

    }
}
