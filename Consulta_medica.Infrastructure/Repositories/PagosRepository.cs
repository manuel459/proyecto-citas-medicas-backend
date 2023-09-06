using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Infrastructure.Enum;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Microsoft.EntityFrameworkCore;

namespace Consulta_medica.Repository
{
    public class PagosRepository : IPagosRepository
    {
        private readonly consulta_medicaContext _context;
        private readonly IGenerarPDF _generarPDF;
        public PagosRepository(consulta_medicaContext context, IGenerarPDF generarPDF) 
        {
            _context = context;
            _generarPDF = generarPDF;
        }

        public async Task<DetailPagoResponse> getInfoPago(int sId_Cita) 
        {

            var cita = await (from c in _context.Citas
                              join p in _context.Paciente
                              on c.Dnip equals p.Dnip
                              join e in _context.Especialidad
                              on c.Codes equals e.Codes
                              join m in _context.Medico
                              on c.Codmed equals m.Codmed
                              where c.Id == sId_Cita && c.nEstado_Pago == (int)GenericEnumRepository.PendientePago
                              select new DetailPagoResponse 
                              {
                                  nDnip = c.Dnip,
                                  sNombre_Paciente = p.Nomp,
                                  sEspecialidad = e.Nombre,
                                  sNombre_Medico = m.Nombre,
                                  dFecha_Cita = c.Feccit,
                                  dImporte_Total = e.Costo
                              }).FirstOrDefaultAsync();

            return cita;
        }

        public async Task<bool> InsertPagoCita(Pagos request) 
        {
            Pagos insert = new() 
            {
                nId_Pago = request.nId_Pago,
                sCod_Cita = request.sCod_Cita,
                nNumero_Tarjeta = request.nNumero_Tarjeta,
                nMes = request.nMes,
                nAnio = request.nAnio,
                nDni = request.nDni,
                dCreate_Datetime = DateTime.Now,
                dImporte_Total = request.dImporte_Total,
                urlBase = request.urlBase
            };
            _context.Pagos.Add(insert);
            var response = await _context.SaveChangesAsync();

            return response > 0; 
        }


        public async Task<bool> UpdateEstadoPagoCita(int nId_Cita)
        {
            var cita = await _context.Citas.FirstOrDefaultAsync(x => x.Id == nId_Cita);

            if (cita is not null)
            {
                cita.nEstado_Pago = 2;
            }

            var response = await _context.SaveChangesAsync();

            return response > 0;
        }

        public string getDocumentPagos(Pagos pagos) 
        {
            var datos = (from c in _context.Citas
                            join e in _context.Especialidad
                            on c.Codes equals e.Codes
                            join m in _context.Medico
                            on c.Codmed equals m.Codmed
                            join p in _context.Paciente
                            on c.Dnip equals p.Dnip
                            join pa in _context.Pagos
                            on c.Id equals pa.sCod_Cita
                            where c.Id == pagos.sCod_Cita
                            select new PagosRequestDto
                            {
                                Nro_Transaction = (int)pa.nId_Pago,
                                Nro_Tarjeta = pagos.nNumero_Tarjeta,
                                Id = c.Id,
                                Dni_Pagador = pagos.nDni,
                                sNombre_Especialidad = e.Nombre,
                                sNombre_Medico = m.Nombre + "" + m.sApellidos,
                                sNombre_Paciente = p.Nomp + " " + p.Apellidos,
                                dImporte_Total = pagos.dImporte_Total
                            }).FirstOrDefault();

            //UPDATE URL BASE PAGO
            var documento = _generarPDF.getDocumentPagos(datos);

            var pago = _context.Pagos.FirstOrDefault(x => x.sCod_Cita == pagos.sCod_Cita);

            pago.urlBase = documento.Item1;

            _context.SaveChanges();

            return documento.Item2;
        }
    }
}
