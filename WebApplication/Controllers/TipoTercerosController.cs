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
    public class TipoTercerosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: TipoTerceros
        public ActionResult Index()
        {
            return View( db.TipoTerceros.Where(a=>a.idTipoTercero!=0).ToList());
        }

        // GET: TipoTerceros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
            if (tipoTerceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoTerceros);
        }

        // GET: TipoTerceros/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: TipoTerceros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoTercero,activo,descripcion")] TipoTerceros tipoTerceros)
        {
            if (ModelState.IsValid)
            {
                 tipoTerceros.activo=true;
                db.TipoTerceros.Add(tipoTerceros);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(tipoTerceros);
        }

        // GET: TipoTerceros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
            if (tipoTerceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoTerceros);
        }

        // POST: TipoTerceros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoTercero,activo,descripcion")] TipoTerceros tipoTerceros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoTerceros).State = EntityState.Modified;
                tipoTerceros.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(tipoTerceros);
        }

        // GET: TipoTerceros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
            if (tipoTerceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoTerceros);
        }

        // POST: TipoTerceros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
             tipoTerceros.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: TipoTerceros/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
            if (tipoTerceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoTerceros);
        }

        // POST: TipoTerceros/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            TipoTerceros tipoTerceros =  db.TipoTerceros.Find(id);
            tipoTerceros.activo=true;
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
