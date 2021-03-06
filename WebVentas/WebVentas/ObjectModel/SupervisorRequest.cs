﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using WebVentas.ObjectRequest;

namespace WebVentas.ObjectModel
{
    public class SupervisorRequest
    {
        public int IdSupervisor { get; set; }

        public string IdUsuario { get; set; }
        public int Idvisita { get; set; }
        public int IdCliente { get; set; }
        public string NombresApellido { get; set; }
        public int IdGerente { get; set; }
        public int IdVendedor { get; set; }
        public bool? Estado { get; set; }
        public string Correo { get; set; }
        public string cliente { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DisplayName("Identificación")]
        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }
        public int IdEmpresa { get; set; }
        public int OpcionMenu { get; set; }
        public List<ClienteRequest> ListarCliente { get; set; }
        public List<VisitaRequest> Listarvisita { get; set; }
        public List<VendedorRequest> ListaVendedores { get; set; }
        public List<VendedorRequest> ListaVendedoresAsignados { get; set; }
        public List<VendedorRequest> ListaVendedoresSinAsignar { get; set; }
        public List<CompromisoRequest> Listarcompromiso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadisticoSupervisorRequest estadisticoSupervisorRequest { get; set; }

    }
}