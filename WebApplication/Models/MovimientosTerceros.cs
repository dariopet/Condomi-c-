using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class MovimientosTerceros
    {
        public int idTercero { get; set; }
        public Egresos egreso { get; set; }
        public decimal totalEgreso { get; set; }
        public Ingresos ingreso { get; set; }
        public decimal totalIngreso { get; set; }   
        public Sueldos sueldo { get; set; }
        public Cobros cobro { get; set; }
        public Pagos pago { get; set; }
        public Adelantos adelanto { get; set; }
        public DateTime fecha { get; set; }
    }
}