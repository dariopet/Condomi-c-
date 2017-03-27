using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class MovimientosCuentas
    {
        public int idCuenta { get; set; }
        public Pagos pago { get; set; }
        public Cobros cobro { get; set; }
        public Transferencias transferencia { get; set; }
        public DateTime fecha { get; set; }
    }
}