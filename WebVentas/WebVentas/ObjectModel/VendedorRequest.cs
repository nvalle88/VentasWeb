using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        // Mapa
        public DateTime FechaRuta { get; set; }

        // Campos de la tabla vendedor
        public int IdVendedor { get; set; }

        public int? TiempoSeguimiento { get; set; } 
        
        public int? IdSupervisor { get; set; }
        public string IdCliente { get; set; }

        [DisplayName("Nombres y apellidos")]
        public string NombreApellido { get; set; }



        //Campos de tabla usuario
        public string IdUsuario { get; set; }

      
        public string TokenContrasena { get; set; }

        
        public string Foto { get; set; }

        public int Estado { get; set; }

        [DisplayName("Contraseña")]
        public string Contrasena { get; set; }

        
        public string Correo { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [DisplayName("Identificación")]
        public string Identificacion { get; set; }

        
        public string Nombres { get; set; }

       
        public string Apellidos { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        public int? idEmpresa { get; set; }


        // Estadisticos
        [DisplayName("Calificación")]
        public int? Calificacion { get; set; }


        public List<ClienteRequest> ListaClientes { get; set; }
        
        public EstadisticoVendedorRequest estadisticoVendedorRequest { get; set; }
    }
}