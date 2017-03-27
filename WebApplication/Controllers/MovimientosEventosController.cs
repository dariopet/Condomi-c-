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
    public class MovimientosEventosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            ViewBag.idEvento = new SelectList(db.Eventos.Where(a => a.activo == true && a.estado != estadoEvento.Presupuestado && a.idEvento!=0), "idEvento", "descripcion");
            return View();
        }

        // GET: Aux
        public  ActionResult PorEvento(int idEvento)
        {
         
            var egresos = db.Egresos.Where(a => a.idEvento == idEvento && a.activo == true);
            var ingresos = db.Ingresos.Where(a => a.idEvento == idEvento && a.activo == true);
            var pagos = db.Pagos.Where(a => a.Egresos.idEvento == idEvento);
            var cobros = db.Cobros.Where(a => a.Ingresos.idEvento == idEvento);

                     
                List<MovimientosEventos> movimientos= new List<MovimientosEventos>();

            foreach (Egresos egr in egresos)
            {
                               
                MovimientosEventos mov=new MovimientosEventos();
                mov.egreso = egr;
                mov.fecha = egr.fechaGenerada;
                movimientos.Add(mov);
                foreach (DetalleEgresos det in egr.DetalleEgresos)
                {
                    mov.totalEgreso += det.monto;
                }
                if (egr.tipoFactura == tiposFacturas.A)
                {
                    var resultadoiva = mov.totalEgreso * (egr.iva / 100);
                    var ingresosbrutos = mov.totalEgreso * (egr.ingresosBrutos / 100);
                    var impuestosinternos = mov.totalEgreso * ((egr.impuestosInternos / 100));
                    var percepcioniva = mov.totalEgreso * ((egr.percepcionIva / 100));

                    mov.totalEgreso = mov.totalEgreso + resultadoiva + ingresosbrutos + impuestosinternos + percepcioniva;

                }
            }
           
            foreach (Ingresos ing in ingresos)
            {
                MovimientosEventos mov = new MovimientosEventos();
                mov.ingreso  = ing;
                mov.fecha = ing.fechaGenerada;
                movimientos.Add(mov);
                foreach (DetalleIngresos det in ing.DetalleIngresos)
                {
                    mov.totalIngreso += det.monto;
                }
                if (ing.tipoFactura == tiposFacturas.A)
                {
                    mov.totalIngreso = mov.totalIngreso * ((ing.iva / 100) + 1);
                }
            }

            foreach (Cobros cob in cobros)
            {
                MovimientosEventos mov = new MovimientosEventos();
                mov.cobro = cob;
                mov.fecha = cob.fecha;
                movimientos.Add(mov);
            }

            foreach (Pagos pag in pagos)
            {
                MovimientosEventos mov = new MovimientosEventos();
                mov.pago = pag;
                mov.fecha = pag.fecha;
                movimientos.Add(mov);
            }          

            return PartialView(movimientos.OrderByDescending(a=>a.fecha));
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
