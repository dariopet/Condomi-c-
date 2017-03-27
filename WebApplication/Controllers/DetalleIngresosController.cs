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
    public class DetalleIngresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: DetalleIngresos/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index(int idIngreso)
        {
            ViewBag.idIngreso = idIngreso;
            var detalleIngresos = db.DetalleIngresos.Include(d => d.Ingresos).Include(d => d.TipoIngresos);
            return View(detalleIngresos.Where(a=>a.idIngreso==idIngreso).ToList());
        }
       

        // GET: DetalleIngresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleIngresos detalleIngresos = db.DetalleIngresos.Find(id);
            if (detalleIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleIngresos);
        }

        // GET: DetalleIngresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create(int idIngreso)
        {
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion");
                                             
            ViewBag.idTipoIngreso = new SelectList(db.TipoIngresos.Where(p => p.activo == true), "idTipoIngreso", "descripcion");

            DetalleIngresos detalleIngreso = new DetalleIngresos();
            detalleIngreso.idIngreso = idIngreso;
            return PartialView(detalleIngreso);
        }

        // POST: DetalleIngresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalleIngreso,idIngreso,monto,idTipoIngreso,cantidad")] DetalleIngresos detalleIngresos)
        {
            if (ModelState.IsValid)
            {
              
                db.DetalleIngresos.Add(detalleIngresos);
                db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", detalleIngresos.idIngreso);
            ViewBag.idTipoIngreso = new SelectList(db.TipoIngresos.Where(p => p.activo == true), "idTipoIngreso", "descripcion", detalleIngresos.idTipoIngreso);
            return PartialView(detalleIngresos);
        }

        // GET: DetalleIngresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleIngresos detalleIngresos = db.DetalleIngresos.Find(id);
            if (detalleIngresos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", detalleIngresos.idIngreso);
            ViewBag.idTipoIngreso = new SelectList(db.TipoIngresos.Where(p => p.activo == true), "idTipoIngreso", "descripcion", detalleIngresos.idTipoIngreso);
            
            return PartialView(detalleIngresos);
        }

        // POST: DetalleIngresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalleIngreso,idIngreso,monto,idTipoIngreso,cantidad")] DetalleIngresos detalleIngresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleIngresos).State = EntityState.Modified;               
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", detalleIngresos.idIngreso);
            ViewBag.idTipoIngreso = new SelectList(db.TipoIngresos.Where(p => p.activo == true), "idTipoIngreso", "descripcion", detalleIngresos.idTipoIngreso);
            return PartialView(detalleIngresos);
        }

        // GET: DetalleIngresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleIngresos detalleIngresos = db.DetalleIngresos.Find(id);
            if (detalleIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleIngresos);
        }

        // POST: DetalleIngresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleIngresos detalleIngresos = db.DetalleIngresos.Find(id);
            db.DetalleIngresos.Remove(detalleIngresos);
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
