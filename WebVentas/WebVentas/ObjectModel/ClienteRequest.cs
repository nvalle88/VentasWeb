using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{
    public class ClienteRequest
    {
        public int IdCliente { get; set; }

        public int? IdEmpresa { get; set; }

        [DisplayName("Razón social")]
        public string RazonSocial { get; set; }

        [DisplayName("Foto")]
        public string Foto { get; set; }

        [DisplayName("Identificación")]
        public string Identificacion { get; set; }

        [DisplayName("Celular")]
        public string TelefonoMovil { get; set; }

        [DisplayName("Firma")]
        public string Firma { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [DisplayName("Télefono convencional")]
        public string Telefono { get; set; }

        [DisplayName("Correo")]
        public string Email { get; set; }

        [DisplayName("Tipo de cliente")]
        public int IdTipoCliente { get; set; }

        public string TipoCliente { get; set; }

        [DisplayName("Vendedor")]
        public int IdVendedor { get; set; }

        [DisplayName("Nombre del vendedor")]
        public string NombresVendedor { get; set; }
        [DisplayName("Apellido del vendedor")]
        public string ApellidosVendedor { get; set; }

        public string Direccion { get; set; }

        public int Estado { get; set; } 

    }
}