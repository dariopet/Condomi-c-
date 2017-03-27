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
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class TipoIngresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: TipoIngresos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            var tipoIngresos = db.TipoIngresos;
            return View( tipoIngresos.ToList());
        }

        // GET: TipoIngresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
            if (tipoIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoIngresos);
        }

        // GET: TipoIngresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
           
            return PartialView();
        }

        // POST: TipoIngresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoIngreso,activo,descripcion")] TipoIngresos tipoIngresos)
        {
            if (ModelState.IsValid)
            {
                 tipoIngresos.activo=true;
                db.TipoIngresos.Add(tipoIngresos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(tipoIngresos);
        }

        // GET: TipoIngresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
            if (tipoIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoIngresos);
        }

        // POST: TipoIngresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoIngreso,activo,descripcion")] TipoIngresos tipoIngresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoIngresos).State = EntityState.Modified;
                tipoIngresos.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(tipoIngresos);
        }

        // GET: TipoIngresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
            if (tipoIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoIngresos);
        }

        // POST: TipoIngresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
             tipoIngresos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: TipoIngresos/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
            if (tipoIngresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoIngresos);
        }

        // POST: TipoIngresos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            TipoIngresos tipoIngresos =  db.TipoIngresos.Find(id);
            tipoIngresos.activo=true;
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
