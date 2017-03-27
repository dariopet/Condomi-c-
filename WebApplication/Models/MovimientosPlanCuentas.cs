using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class MovimientosPlanCuentas
    {
         
        public Ingresos ingreso { get; set; }
        public Egresos egreso { get; set; }
        public Pagos pago { get; set; }
        public Cobros cobro { get; set; }
        public DateTime fecha { get; set; }
    }
}