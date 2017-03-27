using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class MovimientosVinculadoA
    {
       
        public DetalleEgresos detalleEgreso { get; set; }
        public DetalleIngresos detalleIngreso { get; set; }
        public vinculadoARubros vinculado { get; set; }
        public DateTime fecha { get; set; }

        public static implicit operator MovimientosVinculadoA(int v)
        {
            throw new NotImplementedException();
        }
    }
}