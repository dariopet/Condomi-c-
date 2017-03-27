using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SaldoIva
    {
        public Egresos egreso { get; set; }
        public Ingresos ingreso { get; set; }
        public Decimal ivaDebito { get; set; }
        public Decimal ivaCredito { get; set; }
        public DateTime fecha { get; set; }
    }
}