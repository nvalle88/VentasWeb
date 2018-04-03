using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebVentas.ObjectRequest;

namespace WebVentas.ObjectModel
{
    public class VendedorRequest
    {

        // Menú
        public int NumeroMenu { get; set; }


        // Campos de la tabla vendedor
        public int IdVendedor { get; set; }

        public int? TiempoSeguimiento { get; set; } 
        
        public int? IdSupervisor { get; set; }
        public string IdCliente { get; set; }
        public string NombreApellido { get; set; }



        //Campos de tabla usuario
        public string IdUsuario { get; set; }

      
        public string TokenContrasena { get; set; }

        
        public string Foto { get; set; }

        public int Estado { get; set; }

        
        public string Contrasena { get; set; }

        
        public string Correo { get; set; }

       
        public string Direccion { get; set; }

        
        public string Identificacion { get; set; }

        
        public string Nombres { get; set; }

       
        public string Apellidos { get; set; }

        
        public string Telefono { get; set; }

        public int? idEmpresa { get; set; }


        // Estadisticos
        public int? Calificacion { get; set; }


        public List<ClienteRequest> ListaClientes { get; set; }
        
        public EstadisticoVendedorRequest estadisticoVendedorRequest { get; set; }
    }
}