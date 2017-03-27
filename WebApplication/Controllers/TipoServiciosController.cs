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

namespace WebApplication.Controllers
{
    public class TipoServiciosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: TipoServicios
        public ActionResult Index()
        {
            return View( db.TipoServicios.ToList());
        }

        // GET: TipoServicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
            if (tipoServicios == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoServicios);
        }

        // GET: TipoServicios/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: TipoServicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoServicio,activo,descripcion")] TipoServicios tipoServicios)
        {
            if (ModelState.IsValid)
            {
                 tipoServicios.activo=true;
                db.TipoServicios.Add(tipoServicios);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(tipoServicios);
        }

        // GET: TipoServicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
            if (tipoServicios == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoServicios);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoServicio,activo,descripcion")] TipoServicios tipoServicios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoServicios).State = EntityState.Modified;
                tipoServicios.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(tipoServicios);
        }

        // GET: TipoServicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
            if (tipoServicios == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoServicios);
        }

        // POST: TipoServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
             tipoServicios.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: TipoServicios/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
            if (tipoServicios == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoServicios);
        }

        // POST: TipoServicios/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            TipoServicios tipoServicios =  db.TipoServicios.Find(id);
            tipoServicios.activo=true;
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
