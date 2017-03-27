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
    public class MovimientosCuentasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        [AuthLog(Roles = "Administrador, Contabilidad total")]
        public ActionResult Index()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 1), "idCuenta", "descripcion");
            return View();
        }

        // GET: Terceros
        [AuthLog(Roles = "Administrador, Contabilidad total")]
        public ActionResult EntreFechas(String inicioT, String finT, int idcuenta)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);

            var egresos = db.Pagos.Where(a => a.idCuenta == idcuenta && a.fecha >= inicio && a.fecha <= fin);
            var ingresos = db.Cobros.Where(a => a.idCuenta == idcuenta && a.fecha >= inicio && a.fecha <= fin);
            var transferencias = db.Transferencias.Where(a => a.idCuentaOrigen == idcuenta || a.idCuentaDestino==idcuenta && a.fecha >= inicio && a.fecha <= fin);
            Cuentas cuenta = db.Cuentas.Find(idcuenta);
            ViewBag.Cuenta = cuenta.descripcion;

            List<MovimientosCuentas> movimientos= new List<MovimientosCuentas>();

            foreach (Pagos egr in egresos)
            {
                MovimientosCuentas mov=new MovimientosCuentas();
                mov.idCuenta = idcuenta;
                mov.pago = egr;
                mov.fecha = egr.fecha;
                movimientos.Add(mov);
            }
            foreach (Transferencias tra in transferencias)
            {
                MovimientosCuentas mov = new MovimientosCuentas();
                mov.idCuenta = idcuenta;
                mov.transferencia = tra;
                mov.fecha = tra.fecha;
                movimientos.Add(mov);
            }
            foreach (Cobros cob in ingresos)
            {
                MovimientosCuentas mov = new MovimientosCuentas();
                mov.idCuenta = idcuenta;
                mov.cobro  = cob;
                mov.fecha = cob.fecha;
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
