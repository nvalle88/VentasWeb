﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebVentas.ObjectModel
{ 
    public class CompromisoRequest
    {
        public int IdCompromiso { get; set; }

        public int IdTipoCompromiso { get; set; }

        public int  idVisita { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        //public string tipocompromiso { get; set; }
        [DisplayName("Dirección")]
        public string Descripcion { get; set; }

        [DisplayName("Solución")]
        public string Solucion { get; set; }
    }
}