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
    
    public partial class TipoIngresos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoIngresos()
        {
            this.DetalleIngresos = new HashSet<DetalleIngresos>();
        }
    
        public int idTipoIngreso { get; set; }
        public bool activo { get; set; }
        public string descripcion { get; set; }
        public vinculadoARubros vinculadoA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleIngresos> DetalleIngresos { get; set; }
    }
}
