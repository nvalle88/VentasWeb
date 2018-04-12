using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVentas.ObjectModel;

namespace WebVentas.ObjectModel
{
    public class VisitaRecorrido
    {
        public int IdVisita { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public TimeSpan TiempoRecorrido { get; set; }
        public ClienteRequest ClienteRequest { get; set; }
        public List<CompromisosRecorrido> ListaCompromisos { get; set; }
    }

    public class CompromisosRecorrido
    {
        public string TipoCompromiso { get; set; }
        public string Detalle { get; set; }
        public string Solucion { get; set; }
    }
}