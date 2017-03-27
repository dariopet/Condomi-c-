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
    public class DetalleMantenimientoPreventivoController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: DetalleMantenimientoPreventivo
        [AuthLog(Roles = "Administrador, Depósito")]
        public  ActionResult Index(int idMantenimientoPreventivo)
        {
            ViewBag.idMantenimientoPreventivo = idMantenimientoPreventivo;
            var detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Include(d => d.AspNetUsers).Include(d => d.Equipos).Include(d => d.MantenimientoPreventivo);
            return View( detalleMantenimientoPreventivo.Where(a=>a.idMantenimientoPreventivo==idMantenimientoPreventivo).ToList());
        }

        // GET: DetalleMantenimientoPreventivo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            if (detalleMantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleMantenimientoPreventivo);
        }

        // GET: DetalleMantenimientoPreventivo/Create
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Create(int idMantenimientoPreventivo)
        {
            ViewBag.chequeadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
                               
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber");
                                             
            ViewBag.idMantenimientoPreventivo = new SelectList(db.MantenimientoPreventivo.Where(p => p.activo == true), "idMantenimientoPreventivo", "descripcion");

            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = new DetalleMantenimientoPreventivo();
            detalleMantenimientoPreventivo.idMantenimientoPreventivo = idMantenimientoPreventivo;                                  
            return PartialView(detalleMantenimientoPreventivo);
        }

        // POST: DetalleMantenimientoPreventivo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalleMantenimientoPreventivo,idMantenimientoPreventivo,idEquipo,chequeadoPor,fechaChequeado,estado,observaciones")] DetalleMantenimientoPreventivo detalleMantenimientoPreventivo)
        {
            if (ModelState.IsValid)
            {
                
                db.DetalleMantenimientoPreventivo.Add(detalleMantenimientoPreventivo);
               db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.chequeadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", detalleMantenimientoPreventivo.chequeadoPor);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", detalleMantenimientoPreventivo.idEquipo);
            ViewBag.idMantenimientoPreventivo = new SelectList(db.MantenimientoPreventivo.Where(p => p.activo == true), "idMantenimientoPreventivo", "descripcion", detalleMantenimientoPreventivo.idMantenimientoPreventivo);
            return PartialView(detalleMantenimientoPreventivo);
        }

        // GET: DetalleMantenimientoPreventivo/Edit/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            if (detalleMantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.chequeadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", detalleMantenimientoPreventivo.chequeadoPor);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", detalleMantenimientoPreventivo.idEquipo);
            ViewBag.idMantenimientoPreventivo = new SelectList(db.MantenimientoPreventivo.Where(p => p.activo == true), "idMantenimientoPreventivo", "descripcion", detalleMantenimientoPreventivo.idMantenimientoPreventivo);
            return PartialView(detalleMantenimientoPreventivo);
        }

        // POST: DetalleMantenimientoPreventivo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalleMantenimientoPreventivo,idMantenimientoPreventivo,idEquipo,chequeadoPor,fechaChequeado,estado,observaciones")] DetalleMantenimientoPreventivo detalleMantenimientoPreventivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleMantenimientoPreventivo).State = EntityState.Modified;               
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.chequeadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", detalleMantenimientoPreventivo.chequeadoPor);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", detalleMantenimientoPreventivo.idEquipo);
            ViewBag.idMantenimientoPreventivo = new SelectList(db.MantenimientoPreventivo.Where(p => p.activo == true), "idMantenimientoPreventivo", "descripcion", detalleMantenimientoPreventivo.idMantenimientoPreventivo);
            return PartialView(detalleMantenimientoPreventivo);
        }

        // GET: DetalleMantenimientoPreventivo/Delete/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            if (detalleMantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleMantenimientoPreventivo);
        }

        // POST: DetalleMantenimientoPreventivo/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo =  db.DetalleMantenimientoPreventivo.Find(id);
            db.DetalleMantenimientoPreventivo.Remove(detalleMantenimientoPreventivo);
            db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: DetalleMantenimientoPreventivo/Reparandose/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Reparandose(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            if (detalleMantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleMantenimientoPreventivo);
        }

        // POST: DetalleMantenimientoPreventivo/Reparandose/5
        [HttpPost, ActionName("Reparandose")]
        //[ValidateAntiForgeryToken]
        public ActionResult ReparandoseConfirmed(int id)
        {
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            detalleMantenimientoPreventivo.estado = estadoMantenimientoPreventivo.Reparándose;
            db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: DetalleMantenimientoPreventivo/Terminado/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Terminado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            if (detalleMantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(detalleMantenimientoPreventivo);
        }

        // POST: DetalleMantenimientoPreventivo/Terminado/5
        [HttpPost, ActionName("Terminado")]
        //[ValidateAntiForgeryToken]
        public ActionResult TerminadoConfirmed(int id)
        {
            DetalleMantenimientoPreventivo detalleMantenimientoPreventivo = db.DetalleMantenimientoPreventivo.Find(id);
            detalleMantenimientoPreventivo.estado = estadoMantenimientoPreventivo.Terminado;
            detalleMantenimientoPreventivo.chequeadoPor= User.Identity.GetUserId();
            detalleMantenimientoPreventivo.fechaChequeado = DateTime.Now;
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
