using Consulta_medica.Dto.Request;
using Consulta_medica.Infrastructure.Enum;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Consulta_medica.Repository
{
    public class DiagnosticoRepository : IDiagnosticoRepository
    {
        private readonly consulta_medicaContext _context;
        public DiagnosticoRepository(consulta_medicaContext context)
        {
            _context = context;
        }

        public async Task<bool> getUpdDiagnostico(DiagnosticoRequestPdfDto request, List<IFormFile> files) 
        {
            var list = await _context.Citas.Where(x => x.Id == request.idCita).ToListAsync();

            foreach (var item in list)
            {
                item.Estado = (int)GenericEnumRepository.Atendido;
                await _context.SaveChangesAsync();
            }

            HistorialMedico ohistoria = new HistorialMedico();
            ohistoria.idCita = request.idCita;
            ohistoria.Dnip = request.DniPaciente;
            ohistoria.Codmed = request.Codmed;
            ohistoria.Codes = request.Codes;
            ohistoria.Fecct = request.fecct;
            ohistoria.Diagnostico = request.diagnostico;
            ohistoria.Receta = request.medicamentos;
            _context.HistorialMedico.Add(ohistoria);
            var recordAffected = await _context.SaveChangesAsync();


            return recordAffected > 0;
        }

        public int getIdHistoricMedik(int nId_Cita) 
        {
            var result = _context.HistorialMedico.FirstOrDefault(x => x.idCita == nId_Cita);
            return result.Id;
        }

    }
}
