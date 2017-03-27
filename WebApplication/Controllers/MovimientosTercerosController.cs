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
    public class MovimientosTercerosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Terceros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre");
            return View();
        }


        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public  ActionResult EntreFechas(String inicioT, String finT,int idtercero)
        {

            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);

            var egresos = db.Egresos.Where(a => a.idTercero == idtercero && a.activo == true && a.fechaGenerada>=inicio && a.fechaGenerada<=fin);
            var ingresos = db.Ingresos.Where(a => a.idTercero == idtercero && a.activo == true && a.fechaGenerada >= inicio && a.fechaGenerada <= fin);
            var sueldos = db.Sueldos.Where(a => a.idTercero == idtercero && a.fecha >= inicio && a.fecha <= fin);
            var pagos = db.Pagos.Where(a => a.Egresos.idTercero == idtercero && a.fecha >= inicio && a.fecha <= fin);
            var cobros = db.Cobros.Where(a => a.Ingresos.idTercero == idtercero && a.fecha >= inicio && a.fecha <= fin);
            var adelantos = db.Adelantos.Where(a => a.idTercero == idtercero && a.fecha >= inicio && a.fecha <= fin);

            ViewBag.tercero = db.Terceros.Find(idtercero).nombre;
                     
                List<MovimientosTerceros> movimientos= new List<MovimientosTerceros>();

            foreach (Egresos egr in egresos)
            {
               
                
                MovimientosTerceros mov=new MovimientosTerceros();
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
            foreach (Sueldos sue in sueldos)
            {
                MovimientosTerceros mov = new MovimientosTerceros();
                mov.sueldo = sue;
                mov.fecha = sue.fecha;
                movimientos.Add(mov);
            }
            foreach (Ingresos ing in ingresos)
            {
                MovimientosTerceros mov = new MovimientosTerceros();
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
                MovimientosTerceros mov = new MovimientosTerceros();
                mov.cobro = cob;
                mov.fecha = cob.fecha;
                movimientos.Add(mov);
            }

            foreach (Pagos pag in pagos)
            {
                MovimientosTerceros mov = new MovimientosTerceros();
                mov.pago = pag;
                mov.fecha = pag.fecha;
                movimientos.Add(mov);
            }

            foreach (Adelantos ade in adelantos)
            {
                MovimientosTerceros mov = new MovimientosTerceros();
                mov.adelanto = ade;
                mov.fecha = ade.fecha;
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
