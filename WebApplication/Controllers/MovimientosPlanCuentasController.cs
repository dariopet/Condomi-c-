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
    public class MovimientosPlanCuentasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Terceros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public  ActionResult Index()
        {
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion");
            return View();          
        }

        public ActionResult EntreFechas(String inicioT, String finT, Int32 idPlan)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);

            IEnumerable<Ingresos> ingresos ;
            IEnumerable<Egresos> egresos;

            //si no es plan de cuentas "proveedores"
            if (idPlan != 0)
            {
                ingresos = db.Ingresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin && a.activo==true && a.PlanCuentas.idPlanCuentas==idPlan);
                egresos = db.Egresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin && a.activo == true && a.PlanCuentas.idPlanCuentas == idPlan);
            }
            else//listo todos los egresos e ingresos  qno sean de condomi
            {
                ingresos =db.Ingresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin && a.activo == true);
                egresos= db.Egresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin && a.activo == true);
            }
            var pagos = db.Pagos.Where(a => a.Cuentas.idPlanCuentas == idPlan && a.fecha >= inicio && a.fecha <= fin);
            var cobros = db.Cobros.Where(a => a.Cuentas.idPlanCuentas == idPlan && a.fecha >= inicio && a.fecha <= fin);


            List<MovimientosPlanCuentas> movimientos = new List<MovimientosPlanCuentas>();


            foreach (Ingresos ing in ingresos)
            {
               
                    MovimientosPlanCuentas mov = new MovimientosPlanCuentas();
                    mov.ingreso = ing;
                    mov.fecha = ing.fechaGenerada;
                    movimientos.Add(mov);              

                foreach (Cobros cob in ing.Cobros)
                {
                   
                        MovimientosPlanCuentas mov2 = new MovimientosPlanCuentas();
                        mov2.cobro = cob;
                        mov2.fecha = cob.Ingresos.fechaGenerada;
                        movimientos.Add(mov2);
                    
                }
            }

                foreach (Egresos egr in egresos)
                {
                                   
                        MovimientosPlanCuentas mov = new MovimientosPlanCuentas();
                        mov.egreso = egr;
                        mov.fecha = egr.fechaGenerada;
                        movimientos.Add(mov);


                foreach (Pagos pag in egr.Pagos)
                {

                    MovimientosPlanCuentas mov2 = new MovimientosPlanCuentas();
                    mov2.pago = pag;
                    mov2.fecha = pag.Egresos.fechaGenerada;
                    movimientos.Add(mov2);
                }

                    
                }

            foreach (Pagos pag in pagos)
            {
                MovimientosPlanCuentas mov = new MovimientosPlanCuentas();
                mov.pago = pag;
                mov.fecha = pag.Egresos.fechaGenerada;
                movimientos.Add(mov);
            }
            foreach (Cobros cob in cobros)
            {
                MovimientosPlanCuentas mov = new MovimientosPlanCuentas();
                mov.cobro = cob;
                mov.fecha = cob.Ingresos.fechaGenerada;
                movimientos.Add(mov);
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
