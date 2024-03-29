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
    
    public partial class Eventos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Eventos()
        {
            this.MovimientosEquipos = new HashSet<MovimientosEquipos>();
            this.Presupuestos = new HashSet<Presupuestos>();
            this.HorasNoTrabajadas = new HashSet<HorasNoTrabajadas>();
            this.HorasTrabajadasTecnico = new HashSet<HorasTrabajadasTecnico>();
            this.Ingresos = new HashSet<Ingresos>();
            this.Egresos = new HashSet<Egresos>();
        }
    
        public int idEvento { get; set; }
        public bool activo { get; set; }
        public string descripcion { get; set; }
        public string observaciones { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFin { get; set; }
        public string idUsuario { get; set; }
        public estadoEvento estado { get; set; }
        public int idTercero { get; set; }
        public string jurisdiccion { get; set; }
        public string editadoPor { get; set; }
        public Nullable<System.DateTime> fechaEditado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Terceros Terceros { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovimientosEquipos> MovimientosEquipos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorasNoTrabajadas> HorasNoTrabajadas { get; set; }
        public virtual AspNetUsers Editado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorasTrabajadasTecnico> HorasTrabajadasTecnico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ingresos> Ingresos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Egresos> Egresos { get; set; }
    }
}
