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
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class CuentasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Cuentas
        [AuthLog(Roles = "Administrador, Contabilidad total")]
        public ActionResult Index()
        {
            var cuentas = db.Cuentas.Include(c => c.Terceros).Include(c => c.TipoCuentas);
            return View( cuentas.Where(a=>a.idCuenta!=0).ToList());
        }



        // GET: Cuentas/Create

        [AuthLog(Roles = "Administrador, Contabilidad total")]
        public ActionResult Create()
        {
            //ViewBag.idTercero = new SelectList(db.Terceros, "idTercero", "nombre");
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo  == true), "idTercero", "nombre");
            ViewBag.idTipoCuenta = new SelectList(db.TipoCuentas, "idTipoCuenta", "descripcion");
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion");

            return PartialView();
        }

        // POST: Cuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCuenta,fondos,descripcion,CBU,banco,numero,idTipoCuenta,idTercero,idPlanCuentas")] Cuentas cuentas)
        {
            if (ModelState.IsValid)
            {
               
                db.Cuentas.Add(cuentas);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", cuentas.idPlanCuentas);

            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre");
            ViewBag.idTipoCuenta = new SelectList(db.TipoCuentas, "idTipoCuenta", "descripcion", cuentas.idTipoCuenta);
            return PartialView(cuentas);
        }

        // GET: Cuentas/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuentas cuentas =  db.Cuentas.Find(id);
            if (cuentas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", cuentas.idPlanCuentas);

            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre");
            ViewBag.idTipoCuenta = new SelectList(db.TipoCuentas, "idTipoCuenta", "descripcion", cuentas.idTipoCuenta);
            return PartialView(cuentas);
        }

        // POST: Cuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCuenta,fondos,descripcion,CBU,banco,numero,idTipoCuenta,idTercero,idPlanCuentas")] Cuentas cuentas)
        {
            if (ModelState.IsValid)
            {
                cuentas.editadoPor = User.Identity.GetUserId();
                cuentas.fechaEditado = DateTime.Now;
                db.Entry(cuentas).State = EntityState.Modified;
                
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", cuentas.idPlanCuentas);

            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre");
            ViewBag.idTipoCuenta = new SelectList(db.TipoCuentas, "idTipoCuenta", "descripcion", cuentas.idTipoCuenta);
            return PartialView(cuentas);
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
