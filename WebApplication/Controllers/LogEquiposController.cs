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
    
    public class LogEquiposController : Controller
    {
     
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: LogEquipos
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Index()
        {
            var logEquipos = db.LogEquipos.Include(l => l.AspNetUsers).Include(l => l.Equipos);
            return View( logEquipos.OrderByDescending(a => a.fecha).ToList());
        }

        // GET: LogEquipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEquipos logEquipos =  db.LogEquipos.Find(id);
            if (logEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(logEquipos);
        }

        // GET: LogEquipos/Create/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Create(int id)
        {
           
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "codigoBarras");

            LogEquipos logEquipos = new LogEquipos();
            Equipos equipo = db.Equipos.Find(id);
            logEquipos.idEquipo =equipo.idEquipo;
            logEquipos.fecha = DateTime.Now;
           
            logEquipos.Equipos = equipo;

            return PartialView(logEquipos);
        }

        // POST: LogEquipos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idLogEquipos,idEquipo,fecha,descripcion,idUsuario")] LogEquipos logEquipos)
        {
            if (ModelState.IsValid)
            {
                logEquipos.idUsuario = User.Identity.GetUserId();
                db.LogEquipos.Add(logEquipos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logEquipos.idUsuario);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", logEquipos.idEquipo);
            return PartialView(logEquipos);
        }

        // GET: LogEquipos/Edit/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEquipos logEquipos =  db.LogEquipos.Find(id);
            if (logEquipos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logEquipos.idUsuario);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", logEquipos.idEquipo);
            return PartialView(logEquipos);
        }

        // POST: LogEquipos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idLogEquipos,idUsuario, idEquipo, fecha,descripcion")] LogEquipos logEquipos)
        {
            //LogEquipos log = db.LogEquipos.Find(logEquipos.idLogEquipos);
            //logEquipos.idEquipo = log.idEquipo;
            //logEquipos.idUsuario = log.idUsuario;
            //logEquipos.AspNetUsers = log.AspNetUsers;
            //logEquipos.Equipos = log.Equipos;
            if (ModelState.IsValid)
            {
                db.Entry(logEquipos).State = EntityState.Modified;
                logEquipos.editadoPor = User.Identity.GetUserId();
                logEquipos.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logEquipos.idUsuario);
            ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "serialNumber", logEquipos.idEquipo);
            return PartialView(logEquipos);
        }

        // GET: LogEquipos/Delete/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEquipos logEquipos =  db.LogEquipos.Find(id);
            if (logEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(logEquipos);
        }

        // POST: LogEquipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogEquipos logEquipos =  db.LogEquipos.Find(id);
            db.LogEquipos.Remove(logEquipos);
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
