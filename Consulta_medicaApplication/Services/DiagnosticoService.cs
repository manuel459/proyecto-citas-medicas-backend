using Consulta_medica.Application.Interfaces;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Consulta_medica.Application.Services
{
    public class DiagnosticoService : IDiagnosticoService
    {
        public readonly IUnitOfWork _unitOfWork;

        public DiagnosticoService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> getUpdDiagnostico(DiagnosticoRequestPdfDto request, List<IFormFile> files) 
        {
            Response orespuesta = new Response();
            try
            {

                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var response = await _unitOfWork.Diagnostico.getUpdDiagnostico(request, files);

                    if (response)
                    {
                        int nId_Historia = _unitOfWork.Diagnostico.getIdHistoricMedik(request.idCita);

                        var result = _unitOfWork.generarPDF.FileGenericGenerate("HistoriaMedica", nId_Historia, files);

                        orespuesta.exito = 1;
                        orespuesta.mensaje = "Paciente atendido!";
                        orespuesta.data = response;

                        transaction.Complete();
                    }
                    else
                    {
                        orespuesta.mensaje = "Ocurrio un error al momento de generar el diagnostico";
                        orespuesta.data = response;
                    }
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
