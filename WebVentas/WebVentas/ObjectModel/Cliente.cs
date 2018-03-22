namespace WebVentas.ObjectModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Cliente
    {
        
        [Key]
        [StringLength(10)]
        public string idCliente { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        [StringLength(250)]
        public string Firma { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Apellido { get; set; }

        [StringLength(10)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public int? idTipoCliente { get; set; }

        public int IdVendedor { get; set; }

        
    }
}
