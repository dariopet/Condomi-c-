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

namespace WebApplication.Controllers
{
    public class EventosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Eventos
        public ActionResult Index()
        {
            var eventos = db.Eventos.Include(e => e.AspNetUsers).Include(e => e.Terceros);
            return View( eventos.Where(e=>e.idEvento!=0).OrderByDescending(e=> e.fechaInicio).ToList());
        }

        // GET: Eventos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos =  db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return PartialView(eventos);
        }

        // GET: Eventos/Create
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero!=0), "idTercero", "nombre").OrderBy(p => p.Text);
                                             
            return PartialView();
        }

        // POST: Eventos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEvento,activo,descripcion,observaciones,fechaInicio,fechaFin,estado,jurisdiccion,idTercero")] Eventos eventos)
        {
            if (ModelState.IsValid)
            {
                 eventos.activo=true;
                eventos.idUsuario = User.Identity.GetUserId();
                db.Eventos.Add(eventos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", eventos.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", eventos.idTercero).OrderBy(p => p.Text);
            return PartialView(eventos);
        }

        // GET: Eventos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos =  db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", eventos.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", eventos.idTercero).OrderBy(p => p.Text);
            return PartialView(eventos);
        }

        // POST: Eventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEvento,activo,descripcion,observaciones,fechaInicio,fechaFin,idUsuario,estado,jurisdiccion,idTercero")] Eventos eventos)
        {
            if (ModelState.IsValid)
            {

                db.Entry(eventos).State = EntityState.Modified;
                eventos.editadoPor = User.Identity.GetUserId();
                eventos.fechaEditado = DateTime.Now;
                eventos.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", eventos.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", eventos.idTercero).OrderBy(p => p.Text);
            return PartialView(eventos);
        }

        // GET: Eventos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos =  db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return PartialView(eventos);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Eventos eventos =  db.Eventos.Find(id);
             eventos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Eventos/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos eventos =  db.Eventos.Find(id);
            if (eventos == null)
            {
                return HttpNotFound();
            }
            return PartialView(eventos);
        }

        // POST: Eventos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Eventos eventos =  db.Eventos.Find(id);
            eventos.activo=true;
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
