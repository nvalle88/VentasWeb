namespace  WebVentas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Empresa")]
    public partial class Empresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empresa()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Producto = new HashSet<Producto>();
            TipoVisita = new HashSet<TipoVisita>();
            Noticia = new HashSet<Noticia>();
            Suscripcion = new HashSet<Suscripcion>();
            TipoCliente = new HashSet<TipoCliente>();
        }

        [Key]
        public int IdEmpresa { get; set; }

        [Required]
        [StringLength(10)]
        public string Telefono { get; set; }

        [StringLength(13)]
        public string Ruc { get; set; }

        [Required]
        [StringLength(200)]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Producto> Producto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoVisita> TipoVisita { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noticia> Noticia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suscripcion> Suscripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoCliente> TipoCliente { get; set; }
    }
}
