using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVentas.ObjectRequest;

namespace WebVentas.ObjectModel
{
    public class EstadisticoSupervisorRequest
    {
        // Campos de la tabla vendedor
        public int IdSupervisor { get; set; }

        public int? CalificacionPromedio { get; set; }

        public int? CompromisosCumplidos { get; set; }

        public int? CompromisosIncumplidos { get; set; }

        public List<TipoCompromisoRequest> ListaTipoCompromiso { get; set; }
    }
}