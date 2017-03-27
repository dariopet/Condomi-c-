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
    public class ModelosEquiposController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: ModelosEquipos
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Index()
        {
            var modelosEquipos = db.ModelosEquipos.Include(m => m.TipoServicios);
            return View( modelosEquipos.ToList());
        }

         public JsonResult getPreciosModelosEquipos(int idModelo)
        {
            List<ModelosEquipos> modelosEquipos = new List<ModelosEquipos>();
            modelosEquipos = db.ModelosEquipos.Where(a => a.idModelo==idModelo).ToList();
            return new JsonResult { Data = JsonConvert.SerializeObject(modelosEquipos), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       

        // GET: ModelosEquipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
            if (modelosEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(modelosEquipos);
        }

        // GET: ModelosEquipos/Create
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Create()
        {
            ViewBag.idTipoServicio = new SelectList(db.TipoServicios.Where(p => p.activo == true), "idTipoServicio", "descripcion");
                                             
            return PartialView();
        }

        // POST: ModelosEquipos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idModelo,activo,descripcion,costoHora,costoDia,idTipoServicio,precioHora,precioDia")] ModelosEquipos modelosEquipos)
        {
            if (ModelState.IsValid)
            {
                 modelosEquipos.activo=true;
                db.ModelosEquipos.Add(modelosEquipos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idTipoServicio = new SelectList(db.TipoServicios.Where(p => p.activo == true), "idTipoServicio", "descripcion", modelosEquipos.idTipoServicio);
            return PartialView(modelosEquipos);
        }

        // GET: ModelosEquipos/Edit/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
            if (modelosEquipos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoServicio = new SelectList(db.TipoServicios.Where(p => p.activo == true), "idTipoServicio", "descripcion", modelosEquipos.idTipoServicio);
            return PartialView(modelosEquipos);
        }

        // POST: ModelosEquipos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idModelo,activo,descripcion,costoHora,costoDia,idTipoServicio,precioHora,precioDia")] ModelosEquipos modelosEquipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelosEquipos).State = EntityState.Modified;
                modelosEquipos.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idTipoServicio = new SelectList(db.TipoServicios.Where(p => p.activo == true), "idTipoServicio", "descripcion", modelosEquipos.idTipoServicio);
            return PartialView(modelosEquipos);
        }

        // GET: ModelosEquipos/Delete/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
            if (modelosEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(modelosEquipos);
        }

        // POST: ModelosEquipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
             modelosEquipos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: ModelosEquipos/Activate/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
            if (modelosEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(modelosEquipos);
        }

        // POST: ModelosEquipos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            ModelosEquipos modelosEquipos =  db.ModelosEquipos.Find(id);
            modelosEquipos.activo=true;
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
