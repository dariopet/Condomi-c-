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
    public class PlanCuentasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: PlanCuentas
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            return View( db.PlanCuentas.ToList());
        }

        // GET: PlanCuentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
            if (PlanCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(PlanCuentas);
        }

        // GET: PlanCuentas/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: PlanCuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPlanCuentas,activo,descripcion")] PlanCuentas PlanCuentas)
        {
            if (ModelState.IsValid)
            {
                 PlanCuentas.activo=true;
                db.PlanCuentas.Add(PlanCuentas);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(PlanCuentas);
        }

        // GET: PlanCuentas/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
            if (PlanCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(PlanCuentas);
        }

        // POST: PlanCuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPlanCuentas,activo,descripcion")] PlanCuentas PlanCuentas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(PlanCuentas).State = EntityState.Modified;
                PlanCuentas.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(PlanCuentas);
        }

        // GET: PlanCuentas/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
            if (PlanCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(PlanCuentas);
        }

        // POST: PlanCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
             PlanCuentas.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: PlanCuentas/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
            if (PlanCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(PlanCuentas);
        }

        // POST: PlanCuentas/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            PlanCuentas PlanCuentas =  db.PlanCuentas.Find(id);
            PlanCuentas.activo=true;
             db.SaveChanges();
            return Json(new { success = true });
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
