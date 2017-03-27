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
using Newtonsoft.Json;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class TipoEgresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: TipoEgresos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            var tipoEgresos = db.TipoEgresos;
            return View( tipoEgresos.ToList());
        }
        
        // GET: TipoEgresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
            if (tipoEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoEgresos);
        }

        // GET: TipoEgresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
                                             
          
            return PartialView();
        }

        // POST: TipoEgresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoEgreso,activo,descripcion,limiteRetencion,porcentajeRetencion, vinculadoA,codigoRetencion,conceptoRetencion")] TipoEgresos tipoEgresos)
        {
            if (ModelState.IsValid)
            {
                 tipoEgresos.activo=true;
                db.TipoEgresos.Add(tipoEgresos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(tipoEgresos);
        }

        // GET: TipoEgresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
            if (tipoEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoEgresos);
        }

        // POST: TipoEgresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoEgreso,activo,descripcion,limiteRetencion,porcentajeRetencion, vinculadoA,codigoRetencion,conceptoRetencion")] TipoEgresos tipoEgresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEgresos).State = EntityState.Modified;
                tipoEgresos.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(tipoEgresos);
        }

        // GET: TipoEgresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
            if (tipoEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoEgresos);
        }

        // POST: TipoEgresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
             tipoEgresos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: TipoEgresos/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
            if (tipoEgresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoEgresos);
        }

        // POST: TipoEgresos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            TipoEgresos tipoEgresos =  db.TipoEgresos.Find(id);
            tipoEgresos.activo=true;
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
