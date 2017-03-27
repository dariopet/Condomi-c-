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
    public class TipoCuentasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: TipoCuentas
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            return View( db.TipoCuentas.ToList());
        }

        // GET: TipoCuentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
            if (tipoCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoCuentas);
        }

        // GET: TipoCuentas/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: TipoCuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoCuenta,activo,descripcion")] TipoCuentas tipoCuentas)
        {
            if (ModelState.IsValid)
            {
                 tipoCuentas.activo=true;
                db.TipoCuentas.Add(tipoCuentas);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(tipoCuentas);
        }

        // GET: TipoCuentas/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
            if (tipoCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoCuentas);
        }

        // POST: TipoCuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoCuenta,activo,descripcion")] TipoCuentas tipoCuentas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoCuentas).State = EntityState.Modified;
                tipoCuentas.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(tipoCuentas);
        }

        // GET: TipoCuentas/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
            if (tipoCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoCuentas);
        }

        // POST: TipoCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
             tipoCuentas.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: TipoCuentas/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
            if (tipoCuentas == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoCuentas);
        }

        // POST: TipoCuentas/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            TipoCuentas tipoCuentas =  db.TipoCuentas.Find(id);
            tipoCuentas.activo=true;
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
