using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas.Utils
{
    public static class ListaClientesEstados
    {
        public static List<EstadoClienteNombre> ListaEstados { get; set; }
    }

    public class EstadoClienteNombre
    {
        public int IdEstado { get; set; }
        public string Nombre { get; set; }
    }
}