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
    
    public partial class Equipos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipos()
        {
            this.LogEquipos = new HashSet<LogEquipos>();
            this.MovimientosEquipos = new HashSet<MovimientosEquipos>();
            this.DetalleMantenimientoPreventivo = new HashSet<DetalleMantenimientoPreventivo>();
        }
    
        public int idEquipo { get; set; }
        public bool activo { get; set; }
        public int idModelo { get; set; }
        public string serialNumber { get; set; }
        public string codigoBarras { get; set; }
        public Estados estado { get; set; }
        public string ubicacion { get; set; }
        public string numero { get; set; }
        public Nullable<System.DateTime> fechaEditado { get; set; }
        public string editadoPor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogEquipos> LogEquipos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovimientosEquipos> MovimientosEquipos { get; set; }
        public virtual ModelosEquipos ModelosEquipos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleMantenimientoPreventivo> DetalleMantenimientoPreventivo { get; set; }
        public virtual AspNetUsers Editado { get; set; }
    }
}
