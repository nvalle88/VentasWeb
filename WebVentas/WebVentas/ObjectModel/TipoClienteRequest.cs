using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class TipoClienteRequest
    {
        public int idTipoCliente { get; set; }

        public string Tipo { get; set; }

        public int IdEmpresa { get; set; }
    }
}