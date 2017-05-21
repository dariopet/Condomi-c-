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
    public class LogCobranzasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: LogCobranzas
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            return View( );
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var logCobranzas = db.LogCobranzas.Include(l => l.AspNetUsers).Include(l => l.Terceros);

            return PartialView(logCobranzas.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));
        }


        // GET: LogCobranzas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
            if (logCobranzas == null)
            {
                return HttpNotFound();
            }
            return PartialView(logCobranzas);
        }

        // GET: LogCobranzas/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero!=0), "idTercero", "nombre").OrderBy(p => p.Text);
            return PartialView();
        }

        // POST: LogCobranzas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idLogCobranza,activo,idUsuario,idTercero,observacion,fecha")] LogCobranzas logCobranzas)
        {
            if (ModelState.IsValid)
            {
                 logCobranzas.activo=true;
                logCobranzas.idUsuario = User.Identity.GetUserId();
                db.LogCobranzas.Add(logCobranzas);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logCobranzas.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", logCobranzas.idTercero).OrderBy(p => p.Text);
            return PartialView(logCobranzas);
        }

        // GET: LogCobranzas/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
            if (logCobranzas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logCobranzas.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", logCobranzas.idTercero).OrderBy(p => p.Text);
            return PartialView(logCobranzas);
        }

        // POST: LogCobranzas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idLogCobranza,activo,idUsuario,idTercero,observacion,fecha")] LogCobranzas logCobranzas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logCobranzas).State = EntityState.Modified;
                logCobranzas.activo=true;
                logCobranzas .editadoPor = User.Identity.GetUserId();
                logCobranzas.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", logCobranzas.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", logCobranzas.idTercero).OrderBy(p => p.Text);
            return PartialView(logCobranzas);
        }

        // GET: LogCobranzas/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
            if (logCobranzas == null)
            {
                return HttpNotFound();
            }
            return PartialView(logCobranzas);
        }

        // POST: LogCobranzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
             logCobranzas.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: LogCobranzas/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
            if (logCobranzas == null)
            {
                return HttpNotFound();
            }
            return PartialView(logCobranzas);
        }

        // POST: LogCobranzas/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            LogCobranzas logCobranzas =  db.LogCobranzas.Find(id);
            logCobranzas.activo=true;
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
