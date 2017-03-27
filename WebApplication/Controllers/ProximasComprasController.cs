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
    public class ProximasComprasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: ProximasCompras
        public ActionResult Index()
        {
            return View( db.ProximasCompras.OrderByDescending(p=>p.prioridad).ToList());
        }
               

        // GET: ProximasCompras/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: ProximasCompras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProximaCompra,descripcion,prioridad,monto")] ProximasCompras proximasCompras)
        {
            if (ModelState.IsValid)
            {
               
                db.ProximasCompras.Add(proximasCompras);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            return PartialView(proximasCompras);
        }

        // GET: ProximasCompras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProximasCompras proximasCompras =  db.ProximasCompras.Find(id);
            if (proximasCompras == null)
            {
                return HttpNotFound();
            }
            return PartialView(proximasCompras);
        }

        // POST: ProximasCompras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProximaCompra,descripcion,prioridad,monto")] ProximasCompras proximasCompras)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proximasCompras).State = EntityState.Modified;               
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            return PartialView(proximasCompras);
        }

        // GET: ProximasCompras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProximasCompras proximasCompras =  db.ProximasCompras.Find(id);
            if (proximasCompras == null)
            {
                return HttpNotFound();
            }
            return PartialView(proximasCompras);
        }

        // POST: ProximasCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProximasCompras proximasCompras =  db.ProximasCompras.Find(id);
            db.ProximasCompras.Remove(proximasCompras);
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
