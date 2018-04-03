﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class VisitaRequest
    {
        public int idVisita { get; set; }

        public int? Calificacion { get; set; }
        public int CantidadClienteTipoCompromiso { get; set; }

        public string Firma { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string identificacion { get; set; }

        public double? Venta { get; set; }

        public DateTime Fecha { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        public string Foto { get; set; }

        public int IdVendedor { get; set; }

        public int idTipoVisita { get; set; }

        public int? idCliente { get; set; }
    }
}