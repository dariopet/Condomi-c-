using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;

namespace WebApplication.Controllers
{
    public class DetallePresupuestoController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: DetallePresupuesto
        public ActionResult Index(int idPresupuesto)
        {
            ViewBag.idPresupuesto = idPresupuesto;
            var detallePresupuesto = db.DetallePresupuesto.Include(d => d.ModelosEquipos).Include(d => d.Presupuestos);
            return View(detallePresupuesto.Where(a=>a.idPresupuesto== idPresupuesto).ToList());
        }

        // GET: DetallePresupuesto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePresupuesto detallePresupuesto = db.DetallePresupuesto.Find(id);
            if (detallePresupuesto == null)
            {
                return HttpNotFound();
            }
            return PartialView(detallePresupuesto);
        }

        // GET: DetallePresupuesto/Create
        public ActionResult Create(int idPresupuesto)
        {
           
            ViewBag.idModeloEquipo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion");
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre");
            ViewBag.idPresupuesto = new SelectList(db.Presupuestos.Where(p => p.activo == true), "idPresupuesto", "descripcion");

            DetallePresupuesto detallePresupuesto = new DetallePresupuesto();
            detallePresupuesto.idPresupuesto  = idPresupuesto;

            return PartialView();
        }

        // POST: DetallePresupuesto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetallePresupuesto,idPresupuesto,idModeloEquipo,cantidad,precioDia,precioHora,costoHora,costoDia,idTercero,horas,dias,activo,observaciones")] DetallePresupuesto detallePresupuesto)
        {
            if (ModelState.IsValid)
            {
              
                db.DetallePresupuesto.Add(detallePresupuesto);
                db.SaveChanges();
                return Json(new { success = true });
                
            }

            ViewBag.idModeloEquipo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", detallePresupuesto.idModeloEquipo);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre", detallePresupuesto.idTercero);
            ViewBag.idPresupuesto = new SelectList(db.Presupuestos.Where(p => p.activo == true), "idPresupuesto", "descripcion", detallePresupuesto.idPresupuesto);
            return PartialView(detallePresupuesto);
        }

        // GET: DetallePresupuesto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePresupuesto detallePresupuesto = db.DetallePresupuesto.Find(id);
            if (detallePresupuesto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idModeloEquipo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", detallePresupuesto.idModeloEquipo);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre", detallePresupuesto.idTercero);
            ViewBag.idPresupuesto = new SelectList(db.Presupuestos.Where(p => p.activo == true), "idPresupuesto", "descripcion", detallePresupuesto.idPresupuesto);
            return PartialView(detallePresupuesto);
        }

        // POST: DetallePresupuesto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetallePresupuesto,idPresupuesto,idModeloEquipo,cantidad,precioDia,precioHora,costoHora,costoDia,idTercero,horas,dias,activo,observaciones")] DetallePresupuesto detallePresupuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePresupuesto).State = EntityState.Modified;
                
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idModeloEquipo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", detallePresupuesto.idModeloEquipo);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true), "idTercero", "nombre", detallePresupuesto.idTercero);
            ViewBag.idPresupuesto = new SelectList(db.Presupuestos.Where(p => p.activo == true), "idPresupuesto", "descripcion", detallePresupuesto.idPresupuesto);
            return PartialView(detallePresupuesto);
        }

        // GET: DetallePresupuesto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePresupuesto detallePresupuesto = db.DetallePresupuesto.Find(id);
            if (detallePresupuesto == null)
            {
                return HttpNotFound();
            }
            return PartialView(detallePresupuesto);
        }

        // POST: DetallePresupuesto/Delete/5
        [HttpPost, ActionName("Delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
         
            DetallePresupuesto detallePresupuesto = db.DetallePresupuesto.Find(id);
            db.DetallePresupuesto.Remove(detallePresupuesto);
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
