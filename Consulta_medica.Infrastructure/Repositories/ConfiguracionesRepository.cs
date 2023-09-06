using Consulta_medica.Dto.Response;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Consulta_medica.Repository
{
    public class ConfiguracionesRepository : IConfiguracionesRepository
    {
        private readonly consulta_medicaContext _context;
        public ConfiguracionesRepository(consulta_medicaContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<ConfiguracionesResponse>> getConfiguraciones(string sEntidad, string sId) 
        {
            var ParamEntidad = new SqlParameter("@sEntidad", sEntidad == null ? DBNull.Value:sEntidad);

            var ParamId = new SqlParameter("@sId", sId == null ? DBNull.Value : sId);

            var response = await _context.Set<ConfiguracionesResponse>().FromSqlRaw("EXECUTE sp_get_configuraciones @sEntidad, @sId", parameters: new[] { ParamEntidad, ParamId }).ToListAsync();

            return response;
        }

        public async Task<IEnumerable<GraficaCitasResponse>> getGraficaCitas()
        {
            var response = await _context.Set<GraficaCitasResponse>().FromSqlRaw("EXECUTE sp_cantidad_citas").ToListAsync();

            return response;
        }

        public async Task<GraficasCitasDisponiblesStructureResponse> getGraficaCitasDisponibles(string codmed, string dFecha_Consulta)
        {
 
            var ParamCodmed = new SqlParameter("@sCod_Med", codmed == null ? DBNull.Value : codmed);

            var ParamFecha = new SqlParameter("@sFecha", dFecha_Consulta == null ? DBNull.Value : dFecha_Consulta);

            var response = await _context.Set<GraficasCitasDisponiblesResponse>().FromSqlRaw("EXECUTE sp_citas_disponibles @sCod_Med, @sFecha", parameters: new[] { ParamCodmed, ParamFecha }).ToListAsync();

            List<GraficasCitasDisponiblesStructureResponse> result = new();

            var listafinal = (from c in response
                              select new GraficasCitasDisponiblesStructureResponse
                              {
                                 cantidadCitasDisponibles = c.Cantidad_Citas_Disponibles,
                                 cantidadCitasAtendidas = c.Cantidad_Citas_Atendidas,
                                 cantidadCitasReservadasPendientes = c.Cantidad_Citas_Reservadas_Pendientes,
                              }).FirstOrDefault();

            return listafinal;
        }

        public async Task<Configs> getConfigById(string Tb_Notification_Activa) 
        {
            var nCodigo = (await _context.configs.FirstOrDefaultAsync(x => x.sTable == "Tb_Notification_Activa"));

            return nCodigo;
        }
    }
}
