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
    
    public partial class HorasNoTrabajadas
    {
        public int idHorasNoTrabajadas { get; set; }
        public int idTercero { get; set; }
        public System.DateTime periodo { get; set; }
        public System.DateTime fecha { get; set; }
        public int cantidad { get; set; }
        public string idUsuario { get; set; }
        public int idEvento { get; set; }
        public string editadoPor { get; set; }
        public Nullable<System.DateTime> fechaEditado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Eventos Eventos { get; set; }
        public virtual Terceros Terceros { get; set; }
        public virtual AspNetUsers Editado { get; set; }
    }
}
