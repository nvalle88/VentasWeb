using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class Supervisor
    {
        public int IdSupervisor { get; set; }

        public int? IdUsuario { get; set; }

        public int IdGerente { get; set; }

        public string Direccion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }
    }
}