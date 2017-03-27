using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class CertificadoRetencion
    {
        public Pagos pago { get; set; }
        public Terceros retenido { get; set; }
        public Terceros condomi { get; set; }     
    }
}