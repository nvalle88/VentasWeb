using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class RutaRequest
    {

        // Campos de la tabla LogRutaVendedor
        public int IdLogRutaVendedor { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
        
        public DateTime? Fecha { get; set; }

        public int IdVendedor { get; set; }

        public int Hora { get; set; }

        public string TipoCompromiso { get; set; }

        public string Detalle { get; set; }

        public string Solucion { get; set; }

        public TimeSpan TiempoRecorrido { get; set; }

    }
}