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
    public class TercerosController : Controller
    {
        //idTipoTercero id 0 = "cualquiera" x eso no hace falta mostrarlo
        //idtercero 0 = "condomi adrian"
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Terceros
        public ActionResult Index()
        {
            var terceros = db.Terceros.Include(t => t.Cuentas).Include(t => t.TipoTerceros);
            return View( terceros.ToList());
        }

        // GET: Terceros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terceros terceros =  db.Terceros.Find(id);
            if (terceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(terceros);
        }

        // GET: Terceros/Create
        public ActionResult Create()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "descripcion");
            ViewBag.idTipoTercero = new SelectList(db.TipoTerceros.Where(a=>a.idTipoTercero!=0 && a.activo == true) , "idTipoTercero", "descripcion");
            return PartialView();
        }

        // POST: Terceros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTercero,activo,nombre,telefono,direccion,cuil,mail,celular,observaciones,saldo,idCuenta,idTipoTercero,localidad,cp")] Terceros terceros)
        {
            if (ModelState.IsValid)
            {
                 terceros.activo=true;
                db.Terceros.Add(terceros);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }
                        
            ViewBag.idTipoTercero = new SelectList(db.TipoTerceros.Where(a => a.idTipoTercero != 0 && a.activo == true), "idTipoTercero", "descripcion", terceros.idTipoTercero);
            return PartialView(terceros);
        }

        // GET: Terceros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terceros terceros =  db.Terceros.Find(id);
            if (terceros == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.idTipoTercero = new SelectList(db.TipoTerceros.Where(a => a.idTipoTercero != 0 && a.activo == true), "idTipoTercero", "descripcion", terceros.idTipoTercero);
            return PartialView(terceros);
        }

        // POST: Terceros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTercero,activo,nombre,telefono,direccion,cuil,mail,celular,observaciones,saldo,idCuenta,idTipoTercero,localidad,cp")] Terceros terceros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terceros).State = EntityState.Modified;
                terceros.activo=true;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }           
            ViewBag.idTipoTercero = new SelectList(db.TipoTerceros.Where(a => a.idTipoTercero != 0 && a.activo == true), "idTipoTercero", "descripcion", terceros.idTipoTercero);
            return PartialView(terceros);
        }

        // GET: Terceros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terceros terceros =  db.Terceros.Find(id);
            if (terceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(terceros);
        }

        // POST: Terceros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Terceros terceros =  db.Terceros.Find(id);
             terceros.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Terceros/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terceros terceros =  db.Terceros.Find(id);
            if (terceros == null)
            {
                return HttpNotFound();
            }
            return PartialView(terceros);
        }

        // POST: Terceros/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Terceros terceros =  db.Terceros.Find(id);
            terceros.activo=true;
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
