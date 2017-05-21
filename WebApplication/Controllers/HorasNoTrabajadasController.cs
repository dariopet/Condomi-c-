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
    public class HorasNoTrabajadasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: HorasNoTrabajadas
        public ActionResult Index()
        {
            var horasNoTrabajadas = db.HorasNoTrabajadas.Include(h => h.AspNetUsers).Include(h => h.Eventos).Include(h => h.Terceros);
            return View( horasNoTrabajadas.OrderByDescending(a => a.fecha).ToList());
        }

        // GET: HorasNoTrabajadas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasNoTrabajadas horasNoTrabajadas =  db.HorasNoTrabajadas.Find(id);
            if (horasNoTrabajadas == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasNoTrabajadas);
        }

        // GET: HorasNoTrabajadas/Create
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");                                             
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true), "idEvento", "descripcion");
            //empleado o tecnico                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true &&(p.idTipoTercero==1 || p.idTipoTercero==2)), "idTercero", "nombre").OrderBy(p => p.Text);
                                             
            return PartialView();
        }

        // POST: HorasNoTrabajadas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idHorasNoTrabajadas,idTercero,periodo,fecha,cantidad,idUsuario,idEvento")] HorasNoTrabajadas horasNoTrabajadas)
        {
            if (ModelState.IsValid)
            {
                horasNoTrabajadas.idUsuario = User.Identity.GetUserId();
                db.HorasNoTrabajadas.Add(horasNoTrabajadas);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasNoTrabajadas.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true), "idEvento", "descripcion", horasNoTrabajadas.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasNoTrabajadas.idTercero).OrderBy(p => p.Text);
            return PartialView(horasNoTrabajadas);
        }

        // GET: HorasNoTrabajadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasNoTrabajadas horasNoTrabajadas =  db.HorasNoTrabajadas.Find(id);
            if (horasNoTrabajadas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasNoTrabajadas.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true), "idEvento", "descripcion", horasNoTrabajadas.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasNoTrabajadas.idTercero).OrderBy(p => p.Text);
            return PartialView(horasNoTrabajadas);
        }

        // POST: HorasNoTrabajadas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idHorasNoTrabajadas,idTercero,periodo,fecha,cantidad,idUsuario,idEvento")] HorasNoTrabajadas horasNoTrabajadas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horasNoTrabajadas).State = EntityState.Modified;
                horasNoTrabajadas .editadoPor = User.Identity.GetUserId();
                horasNoTrabajadas.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasNoTrabajadas.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true), "idEvento", "descripcion", horasNoTrabajadas.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasNoTrabajadas.idTercero).OrderBy(p => p.Text);
            return PartialView(horasNoTrabajadas);
        }

        // GET: HorasNoTrabajadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasNoTrabajadas horasNoTrabajadas =  db.HorasNoTrabajadas.Find(id);
            if (horasNoTrabajadas == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasNoTrabajadas);
        }

        // POST: HorasNoTrabajadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HorasNoTrabajadas horasNoTrabajadas =  db.HorasNoTrabajadas.Find(id);
            db.HorasNoTrabajadas.Remove(horasNoTrabajadas);
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
