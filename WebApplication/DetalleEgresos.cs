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
    
    public partial class DetalleEgresos
    {
        public int idDetalleEgreso { get; set; }
        public int idEgreso { get; set; }
        public decimal monto { get; set; }
        public int idTipoEgreso { get; set; }
        public int cantidad { get; set; }
    
        public virtual TipoEgresos TipoEgresos { get; set; }
        public virtual Egresos Egresos { get; set; }
    }
}
