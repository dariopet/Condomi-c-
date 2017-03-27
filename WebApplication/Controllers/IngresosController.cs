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
using Microsoft.AspNet.Identity;
using System.Globalization;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class IngresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Ingresos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            var ingresos = db.Ingresos.Include(i => i.AspNetUsers).Include(i => i.Eventos).Include(i => i.Terceros).Include(i=>i.Cobros ).Include(t => t.PlanCuentas);
            return View( );
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var ingresos = db.Ingresos.Include(i => i.AspNetUsers).Include(i => i.Eventos).Include(i => i.Terceros).Include(i => i.Cobros);


            return PartialView(ingresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin).OrderByDescending(a=>a.fechaGenerada));

        }

        public ActionResult ExisteFactura(string factura)
        {
            var ingresos = db.Ingresos.Where(a => a.activo == true && a.factura == factura);
            int con = 0;
            string utilizada = "";
            foreach (Ingresos ing in ingresos)
            {
                if (con > 0)
                {
                    utilizada += " y en ";
                }
                utilizada += ing.Terceros.nombre + " el " + ing.fechaGenerada.ToShortDateString() + System.Environment.NewLine;

                con++;
            }
            if (con == 0)
            {
                return Content("");
            }
            else
            {
                return Content("<div class='alert alert-info' role='alert'> Nº de factura utilizada para: " + utilizada + " </div>");
            }
        }

        // GET: Proximos Cobros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult ProximosCobros()
        {
            return View();
        }

        public ActionResult ProximosCobrosPartial(String tope)
        {
            List<Ingresos> listaIngresos = new List<Ingresos>();
            DateTime topef = Convert.ToDateTime(tope);
            var ingreso = db.Ingresos.Where(a => a.fechaLimite <= topef);

            foreach (Ingresos ing in ingreso)
            {
                if (Convert.ToDecimal(getSaldoIngreso(ing.idIngreso)) > 0)
                {
                    listaIngresos.Add(ing);
                }
            }
            return PartialView(listaIngresos.OrderBy(a => a.fechaLimite));
        }



        // GET: Ingresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos =  db.Ingresos.Find(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(ingresos);
        }

        // GET: Ingresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier"); 
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion");
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero!=0), "idTercero", "nombre");
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion");

            Ingresos ing = new Ingresos();
            ing.tipoFactura = tiposFacturas.A;
            ing.factura = (db.Ingresos.OrderByDescending(p => p.idIngreso).FirstOrDefault().idIngreso + 1).ToString();
            return PartialView(ing);
        }

        // POST: Ingresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idIngreso,activo,descripcion,factura,tipoFactura,fechaLimite,fechaGenerada,fechaCobro,idUsuario,idCuenta,idTercero,totalCobrado,enBlanco,iva,idEvento,idPlanCuentas")] Ingresos ingresos)
        {
            if (ModelState.IsValid)
            {
                ingresos.idUsuario= User.Identity.GetUserId();
                ingresos.activo=true;
                db.Ingresos.Add(ingresos);
                 db.SaveChanges();
                int id = ingresos.idIngreso;
                //oculto el modal y redirecciono a la pagina para agregar los servicios
                string cont = "<script>  $('#myModal').modal('hide'); window.location.href = '/Ingresos/Edit/" + id.ToString() + "';  </script>";
                return Content(cont);

            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", ingresos.idPlanCuentas);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", ingresos.idUsuario);        
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", ingresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", ingresos.idTercero);
            return PartialView(ingresos);
        }

        // GET: Ingresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos =  db.Ingresos.Find(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", ingresos.idPlanCuentas);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", ingresos.idUsuario);      
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", ingresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", ingresos.idTercero);
            return PartialView(ingresos);
        }

        // POST: Ingresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIngreso,activo,descripcion,factura,tipoFactura,fechaLimite,fechaGenerada,fechaCobro,idUsuario,idCuenta,idTercero,totalCobrado,enBlanco,iva,idEvento,idPlanCuentas")] Ingresos ingresos)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(ingresos).State = EntityState.Modified;
                ingresos.editadoPor = User.Identity.GetUserId();
                ingresos.fechaEditado = DateTime.Now;
                ingresos.activo=true;
                 db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", ingresos.idPlanCuentas);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", ingresos.idUsuario);         
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", ingresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", ingresos.idTercero);
            return PartialView(ingresos);
        }

        // GET: Ingresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos =  db.Ingresos.Find(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(ingresos);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingresos ingresos =  db.Ingresos.Find(id);
             ingresos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Ingresos/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos =  db.Ingresos.Find(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(ingresos);
        }

        // POST: Ingresos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Ingresos ingresos =  db.Ingresos.Find(id);
            ingresos.activo=true;
             db.SaveChanges();
            return Json(new { success = true });
        }

        //devuelve el total de un Ingreso
        public String getTotalIngreso(int idIngreso)
        {
            decimal total = 0;
            Ingresos ingreso = db.Ingresos.Find(idIngreso);
            foreach (var det in ingreso.DetalleIngresos)
            {
                total += det.cantidad * det.monto;
            }
            if (ingreso.tipoFactura == tiposFacturas.A)//iva discriminado
            {
                var resultadoiva = total * (ingreso.iva / 100);
                total = total + resultadoiva;
            }
            return (total.ToString());
        }

        //devuelve el saldo de un Ingreso
        public String getSaldoIngreso(int idIngreso)
        {
            decimal total = Convert.ToDecimal(getTotalIngreso(idIngreso).ToString());
            decimal cobrado = Convert.ToDecimal(getCobradoIngreso(idIngreso).ToString());
            decimal saldo = total - cobrado;
            return (saldo.ToString());
        }

        //devuelve el total cobrado de un ingreso
        public String getCobradoIngreso(int idIngreso)  {
            Ingresos ingreso = db.Ingresos.Find(idIngreso);
            decimal totalCobrado = 0;
            foreach (var det in ingreso.Cobros.Where(a => a.idIngreso == ingreso.idIngreso))
            {
                totalCobrado += det.monto;
            }
            return (totalCobrado.ToString());
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
