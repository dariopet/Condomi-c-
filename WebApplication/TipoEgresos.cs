//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoEgresos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoEgresos()
        {
            this.DetalleEgresos = new HashSet<DetalleEgresos>();
        }
    
        public int idTipoEgreso { get; set; }
        public bool activo { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> limiteRetencion { get; set; }
        public Nullable<decimal> porcentajeRetencion { get; set; }
        public vinculadoARubros vinculadoA { get; set; }
        public string codigoRetencion { get; set; }
        public string conceptoRetencion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleEgresos> DetalleEgresos { get; set; }
    }
}