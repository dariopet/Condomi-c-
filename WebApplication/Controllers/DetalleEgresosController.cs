using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class DetalleEgresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: DetalleEgresos/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index(int idEgreso)
        {
            ViewBag.idEgreso = idEgreso;
            var detalleEgresos = db.DetalleEgresos.Include(d => d.Egresos).Include(d => d.TipoEgresos);
            return View(detalleEgresos.Where(a=>a.idEgreso==idEgreso ).ToList());
        }

        // GET: DetalleEgresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleEgresos detalleEgresos = db.DetalleEgresos.Find(id);
            if (detalleEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleEgresos);
        }

        // GET: DetalleEgresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create(int idEgreso)
        {
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion");
                                             
            ViewBag.idTipoEgreso = new SelectList(db.TipoEgresos.Where(p => p.activo == true), "idTipoEgreso", "descripcion");

            DetalleEgresos detalleEgreso = new DetalleEgresos();
            detalleEgreso.idEgreso = idEgreso;
            return PartialView(detalleEgreso);
        }

        // POST: DetalleEgresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalleEgreso,idEgreso,monto,idTipoEgreso,cantidad")] DetalleEgresos detalleEgresos)
        {
            if (ModelState.IsValid)
            {
               
                db.DetalleEgresos.Add(detalleEgresos);
                db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", detalleEgresos.idEgreso);
            ViewBag.idTipoEgreso = new SelectList(db.TipoEgresos.Where(p => p.activo == true), "idTipoEgreso", "descripcion", detalleEgresos.idTipoEgreso);
            return PartialView(detalleEgresos);
        }

        // GET: DetalleEgresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleEgresos detalleEgresos = db.DetalleEgresos.Find(id);
            if (detalleEgresos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", detalleEgresos.idEgreso);
            ViewBag.idTipoEgreso = new SelectList(db.TipoEgresos.Where(p => p.activo == true), "idTipoEgreso", "descripcion", detalleEgresos.idTipoEgreso);
            return PartialView(detalleEgresos);
        }

        // POST: DetalleEgresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalleEgreso,idEgreso,monto,idTipoEgreso,cantidad")] DetalleEgresos detalleEgresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleEgresos).State = EntityState.Modified;
                
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", detalleEgresos.idEgreso);
            ViewBag.idTipoEgreso = new SelectList(db.TipoEgresos.Where(p => p.activo == true), "idTipoEgreso", "descripcion", detalleEgresos.idTipoEgreso);
            return PartialView(detalleEgresos);
        }

        // GET: DetalleEgresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleEgresos detalleEgresos = db.DetalleEgresos.Find(id);
            if (detalleEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleEgresos);
        }

        // POST: DetalleEgresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleEgresos detalleEgresos = db.DetalleEgresos.Find(id);
            db.DetalleEgresos.Remove(detalleEgresos);
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
