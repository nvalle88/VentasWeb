using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class CompromisoRequest
    {
        public DateTime FechaInicio{get;set;}
        public DateTime FechaFin { get; set; }
        public int OpcionMenu { get; set; }
    }
}