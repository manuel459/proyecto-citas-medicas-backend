using Consulta_medica.Dto.Request;
using Consulta_medica.Infrastructure.Data;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;


namespace Consulta_medica.Repository
{
    public class GenerarPDF : IGenerarPDF
    {
        private readonly consulta_medicaContext _context;
        public GenerarPDF(consulta_medicaContext context)
        {
            _context = context;
        }

        public string GenerateInvestorDocument(CitasRequestDto contractInfo)
        {
            //Extraction Nombre de la especialidad 
            contractInfo.NombreEspecialidad = _context.Especialidad.Where(x => x.Codes == contractInfo.Codes).Select(x => x.Nombre).FirstOrDefault();
            contractInfo.NombreMedico = _context.Medico.Where(x => x.Codmed == contractInfo.Codmed).Select(x => x.Nombre + " " + x.sApellidos).FirstOrDefault();
            contractInfo.Costo = _context.Especialidad.Where(x => x.Codes == contractInfo.Codes).Select(x => x.Costo).FirstOrDefault();
            //end

            DateTime fecha = DateTime.Now;

            string filePath = @"Template\";
            string fileNameExisting = @"cita_medica.pdf";
            //string fileNameNew = @"cita_medica_" +DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString()+DateTime.Now.Hour+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString()+contractInfo.NombrePaciente+ ".pdf";

            string fileNameNew = $"cita_medica_{fecha.Year}{fecha.Month}{fecha.Day}{fecha.Hour}{fecha.Minute}{fecha.Second}{contractInfo.NombrePaciente}.pdf";

            string fullNewPath = filePath + fileNameNew;
            string fullExistingPath = filePath + fileNameExisting;

            using (var existingFileStream = new FileStream(fullExistingPath, FileMode.Open))

            using (var newFileStream = new FileStream(fullNewPath, FileMode.Create))
            {
                // Open existing PDF
                var pdfReader = new PdfReader(existingFileStream);

                // PdfStamper, which will create
                var stamper = new PdfStamper(pdfReader, newFileStream);

                AcroFields fields = stamper.AcroFields;
                fields.SetField("CodCita", contractInfo.Id.ToString());
                fields.SetField("NombrePaciente", contractInfo.NombrePaciente);
                fields.SetField("Dnip", contractInfo.Dnip.ToString());
                fields.SetField("Nombre", contractInfo.NombreMedico);
                fields.SetField("NombreEspecialidad", contractInfo.NombreEspecialidad);
                fields.SetField("Feccit", contractInfo.Feccit.ToString("dd/M/yyyy") + " " + contractInfo.Hora);
                fields.SetField("Costo", contractInfo.Costo.ToString());


                // "Flatten" the form so it wont be editable/usable anymore
                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();

                return fullNewPath;
            }

        }

        public (string,string) getDocumentPagos(PagosRequestDto pagos)
        {
            DateTime fecha = DateTime.Now;

            int nParteOculta = pagos.Nro_Tarjeta.Length - 4;

            string sCadenaOculta = new string('*', nParteOculta);

            string sCadenaVisible = pagos.Nro_Tarjeta.Substring(nParteOculta);

            pagos.Nro_Tarjeta = sCadenaOculta + sCadenaVisible;

            // Estructura HTML que deseas convertir a PDF
            string html = @"<!DOCTYPE html>
                            <html lang=""es"">
                            <head>
                                <meta charset=""UTF-8"">
                                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                <title>Boleta de Pago</title>
                                <style>
                                    /* Estilos de la boleta de pago (puedes personalizarlos según tus necesidades) */
                                    body {
                                        font-family: Arial, sans-serif;
                                        margin: 0;
                                        padding: 20px;
                                    }
                                    .container {
                                        border: 1px solid #ccc;
                                        padding: 20px;
                                        max-width: 500px;
                                        margin: 0 auto;
                                    }
                                    .header {
                                        text-align: center;
                                        margin-bottom: 20px;
                                    }
                                    .employee-info {
                                        margin-bottom: 20px;
                                    }
                                    .table-container {
                                        width: 100%;
                                    }
                                    table {
                                        width: 100%;
                                        border-collapse: collapse;
                                    }
                                    th, td {
                                        border: 1px solid #ccc;
                                        padding: 8px;
                                        text-align: left;
                                    }
                                    .total {
                                        font-weight: bold;
                                    }
                                    .footer {
                                        text-align: center;
                                        margin-top: 20px;
                                    }
                                </style>
                            </head>
                            <body>
                                <div class=""container"">
                                    <div class=""header"">
                                        <h1>Boleta de Pago</h1>
                                    </div>";

                            html += $"<div class='employee-info'>" +
                                        $"<p><strong>Nro Transacción:</strong>{pagos.Nro_Transaction}</p><p><strong>Código de Cita:</strong>{pagos.Id}</p><p><strong>Nro de Tarjeta:</strong>{pagos.Nro_Tarjeta}</p><p><strong>DNI:</strong>{pagos.Dni_Pagador}</p><p><strong>Fecha de Transacción:</strong>{fecha.ToString("yyyy-MM-dd")}</p></div>" +
                                    "<div class='table-container'>"+
                                        "<table>"+
                                            "<thead>"+
                                                "<tr>"+
                                                    "<th>Concepto</th>"+
                                                    "<th>Especialidad</th>"+
                                                    "<th>Nombre del Médico</th>"+
                                                    "<th>Nombre del Paciente</th>"+
                                                    "<th>Monto</th>"+
                                                "</tr>"+
                                            "</thead>"+
                                            "<tbody>";

                                html += $"<tr><td>Consulta</td><td>{pagos.sNombre_Especialidad}</td><td>{pagos.sNombre_Medico}</td><td>{pagos.sNombre_Paciente}</td><td>S/{pagos.dImporte_Total}</td></tr>";

            html+= "</tbody>"+
                                "<tfoot>"+
                                    "<tr>"+
                                        "<td>Total</td>"+
                                        "<td></td>"+
                                        "<td></td>"+
                                        "<td></td>"+
                                        $"<td>S/{pagos.dImporte_Total}</td>"+
                                    "</tr>"+
                                "</tfoot>"+
                                "</table>"+
                            "</div>"+
                        "</div>"+
                    "</body>"+
                    "</html>";

            // Crear un objeto IronPdf.HtmlToPdf
            HtmlToPdf renderer = new HtmlToPdf();

            // Convertir el HTML a PDF
            IronPdf.PdfDocument pdf = renderer.RenderHtmlAsPdf(html);

            // Ruta donde se guardará el archivo PDF resultante
            string rutaPDF = @$"Template_Pagos\pagos{fecha.Year}{fecha.Month}{fecha.Day}{fecha.Hour}{fecha.Minute}{fecha.Second}.pdf";

            // Guardar el archivo PDF en la ubicación especificada
            pdf.SaveAs(rutaPDF);

            if (!string.IsNullOrEmpty(pagos.sEmail))
            {
                EnvioNotificationGeneric(pagos.sEmail, "BOLETA DE PAGO", "Tu operación se realizo con éxito!", rutaPDF);
            }

            // Obtener el contenido del PDF en bytes
            byte[] contenidoPdfBytes = pdf.BinaryData;

            // Convertir los bytes del PDF a Base64
            string pdfBase64 = System.Convert.ToBase64String(contenidoPdfBytes);

            return (rutaPDF, pdfBase64);
        }

        public string MessageTemplate(int codigo) 
        {
            string message = "";

            switch (codigo)
            {
                case 0:
                    message = "<b>Buen día estimado(a) NOMBRE_PACIENTE adjunto su cita médica<b>";
                    break;
                case 1:
                    message = $"<b>Buen dia estimado(a) NOMBRE_PACIENTE , recordatorio que su cita médica es el FECHA_CITA a las HORA_CITA<b>";
                    break;
                default:
                    break;
            }
            return message;
        }

        public void EnvioNotificationGeneric(string CorreoDestino, string Encabezado, string bodyMessage, string? newDocumentFileName)
        {
            string EmailOrigen = "manuel.chirre.sepulveda@gmail.com";

            string contraseña = "oivengxhqmwvfzle";

            MailMessage mailMessage = new MailMessage(EmailOrigen, CorreoDestino, Encabezado, bodyMessage);

            if (newDocumentFileName != null)
            {
                mailMessage.Attachments.Add(new Attachment(newDocumentFileName));
            }

            mailMessage.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(EmailOrigen, contraseña);
            client.Send(mailMessage);

            client.Dispose();

        }

        public bool FileGenericGenerate(string sEntidad, int nId_Entidad , List<IFormFile> files) 
        {
            int nRecord = 0;
            DateTime fecha = DateTime.Now;
            string rutaPDF = @$"Template_Files";
            foreach (var file in files)
            {
                // Combina la ruta de destino con el nombre del archivo
                string rutaCompleta = Path.Combine(rutaPDF, $"{sEntidad}{fecha.Year}{fecha.Month}{fecha.Day}{fecha.Hour}{fecha.Minute}{fecha.Second}{file.FileName}");

                // Crea un flujo de salida para el archivo
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    // Copia los datos del archivo al flujo de salida
                    file.CopyTo(stream);
                }

                Files oFile = new() 
                {
                    sEntidad = sEntidad,
                    nId_Entidad = nId_Entidad,
                    sUrl = rutaCompleta,
                    sFile_Name = $"{sEntidad}{fecha.Year}{fecha.Month}{fecha.Day}{fecha.Hour}{fecha.Minute}{fecha.Second}{file.FileName}",
                    sType_File = file.ContentType
                };

                _context.Add(oFile);

                nRecord = _context.SaveChanges();
            }

            return nRecord > 0;
        }
    }
}
