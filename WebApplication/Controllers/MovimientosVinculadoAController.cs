using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class MovimientosVinculadoAController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Terceros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public  ActionResult Index()
        {
            return View();          
        }

        public ActionResult EntreFechas(String inicioT, String finT, vinculadoARubros vinculadoA)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);


           

            var egresos = db.DetalleEgresos.Where(a => a.Egresos.fechaGenerada >= inicio && a.Egresos.fechaGenerada <= fin);
            var ingresos = db.DetalleIngresos.Where(a => a.Ingresos.fechaGenerada >= inicio && a.Ingresos.fechaGenerada <= fin);

            List<MovimientosVinculadoA> movimientos = new List<MovimientosVinculadoA>();

            foreach (DetalleEgresos det in egresos)
            {
                if (det.TipoEgresos.vinculadoA == vinculadoA || Convert.ToInt32(vinculadoA)==-1)
                {
                    MovimientosVinculadoA mov = new MovimientosVinculadoA();
                    mov.vinculado = det.TipoEgresos.vinculadoA;
                    mov.detalleEgreso = det;
                    mov.fecha = det.Egresos.fechaGenerada;
                    movimientos.Add(mov);
                }
            }

            foreach (DetalleIngresos det in ingresos)
            {
                if (det.TipoIngresos.vinculadoA == vinculadoA || Convert.ToInt32(vinculadoA) == -1)
                {
                    MovimientosVinculadoA mov = new MovimientosVinculadoA();
                    mov.vinculado = det.TipoIngresos.vinculadoA;
                    mov.detalleIngreso = det;
                    mov.fecha = det.Ingresos.fechaGenerada;
                    movimientos.Add(mov);
                }
            }

            return PartialView(movimientos.OrderByDescending(a => a.fecha));
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
