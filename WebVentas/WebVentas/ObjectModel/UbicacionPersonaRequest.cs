﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebVentas.Models;

namespace WebVentas.ObjectModel
{
    public class UbicacionPersonaRequest
    {
        
        public int? TiempoSeguimiento { get; set; }

        public int? IdCliente { get; set; }

        public int IdVendedor { get; set; }

        public int? IdSupervisor { get; set; }


        //Ubicacion
        public double? Latitud { get; set; }

        public double? Longitud { get; set; }


        //Campos de tabla usuario
        public string IdUsuario { get; set; }

        [StringLength(250)]
        public string TokenContrasena { get; set; }

        [StringLength(500)]
        public string Foto { get; set; }

        public int Estado { get; set; }

        [StringLength(500)]
        public string Contrasena { get; set; }

        [StringLength(80)]
        public string Correo { get; set; }

        [StringLength(100)]
        public string Direccion { get; set; }

        [StringLength(13)]
        public string Identificacion { get; set; }

        [StringLength(200)]
        public string Nombres { get; set; }

        [StringLength(200)]
        public string Apellidos { get; set; }

        [StringLength(10)]
        public string Telefono { get; set; }

        public int? idEmpresa { get; set; }

        public List<LogRutaVendedor> ListaUbicaciones { get; set; }
    }
}