using System.Collections.Generic;

namespace Consulta_medica.Dto.Response
{
    public class GraficaCitasResponse
    {
        public string sNombre_Especialidad { get; set; }
        public int nCantidad_citas { get; set; }
    }

    public class GraficasCitasDisponiblesResponse 
    {
        public string codmed { get; set; }
        public int Cantidad_Citas_Disponibles { get; set; }
        public int Cantidad_Citas_Reservadas_Pendientes { get; set; }
        public int Cantidad_Citas_Atendidas { get; set; }
    }
    public class CantidadCitasDisponibles
    {
        public int Cantidad_Citas_Disponibles { get; set; }
    }
    public class CantidadCitasReservadasPendientes
    {
        public int Cantidad_Citas_Reservadas_Pendientes { get; set; }
    }

    public class CantidadCitasAtendidas
    {
        public int Cantidad_Citas_Atendidas { get; set; }
    }

    public class GraficasCitasDisponiblesStructureResponse
    {
        public int cantidadCitasDisponibles { get; set; }
        public int cantidadCitasReservadasPendientes { get; set; }
        public int cantidadCitasAtendidas { get; set; }
    }
}
