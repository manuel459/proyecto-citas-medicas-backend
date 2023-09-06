using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Repository
{
    public class CitasMedicasReporteRepository : ICitasMedicasReporteRepository
    {
        private readonly consulta_medicaContext _context;
        public CitasMedicasReporteRepository(consulta_medicaContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<CitasMedicasReporteResponse>> getReporte(RequestFilterDtoNew request) 
        {
            var ParamNombreMedico = new SqlParameter("@sNombreMedico", request.sNombreMedico != null ? request.sNombreMedico : DBNull.Value);
            var ParamNombrePaciente = new SqlParameter("@sNombrePaciente", request.sNombrePaciente != null ? request.sNombrePaciente : DBNull.Value);
            var ParamFechaIni = new SqlParameter("@dFechaInicio", request.dFechaInicio != null ? request.dFechaInicio : DBNull.Value);
            var ParamFechaFin = new SqlParameter("@dFechaFin", request.dFechaFin != null ? request.dFechaFin : DBNull.Value);
            var ParamState = new SqlParameter("@nStateFilter", request.nStateFilter != null ? request.nStateFilter : DBNull.Value);

            var queryReponse = await _context.Set<CitasMedicasReporteResponse>().FromSqlRaw("EXECUTE sp_reporte_citas @sNombreMedico, @sNombrePaciente, @dFechaInicio, @dFechaFin, @nStateFilter", parameters: new[] { ParamNombreMedico, ParamNombrePaciente, ParamFechaIni, ParamFechaFin, ParamState }).ToListAsync();
            return queryReponse;
        } 
    }
}
