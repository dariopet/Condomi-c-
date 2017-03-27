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
    public class PresupuestosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Presupuestos?5
        public ActionResult Index(int? id)
        {
            var presupuestos = db.Presupuestos.Include(p => p.AspNetUsers).Include(p => p.Eventos);
            if(id==null)
            {
                ViewBag.unEvento = "no";
                return View( presupuestos.OrderByDescending(a => a.fechaInicio).ToList());
            }
            else
            {
                ViewBag.unEvento = "si";
                ViewBag.descripcion = db.Eventos.Find(id).descripcion;
                ViewBag.idEvento = id;
                return View( presupuestos.Where(a => a.idEvento==id).OrderByDescending(a => a.fechaInicio).ToList());
            }
        
        }

        // GET: Presupuestos?5
        public ActionResult PosiblesIngresos()
        {
            var presupuestos = db.Presupuestos.Where(p=>p.estado==estadoPresupesto.Pendiente && p.activo==true && p.Eventos.activo==true).OrderByDescending(p=>p.probabilidad ).Include(p => p.AspNetUsers).Include(p => p.Eventos);
            return View( presupuestos.ToList());    
        }


        // GET: Presupuestos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
            if (presupuestos == null)
            {
                return HttpNotFound();
            }
            return PartialView(presupuestos);
        }

        // GET: Presupuestos/Create
        public ActionResult Create(int? id)
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.idEvento==id && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Presupuestado)), "idEvento", "descripcion");
            Eventos evento = new Eventos();
            evento = db.Eventos.Find(id);
            ViewBag.fechaInicio = evento.fechaInicio.ToString();
            ViewBag.fechaFin = evento.fechaFin.ToString();
            Presupuestos presupuesto = new Presupuestos();
            presupuesto.probabilidad = probabilidadPresupuesto.Media;
            return PartialView(presupuesto);
        }

        // POST: Presupuestos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPresupuesto,activo,idEvento,descripcion,observaciones,fechaInicio,fechaFin,idUsuario,estado, probabilidad, bonificacion")] Presupuestos presupuestos)
        {
            if (ModelState.IsValid)
            {
                if (presupuestos.bonificacion==null)
                {
                    presupuestos.bonificacion = 0;
                }
                 presupuestos.activo=true;
                presupuestos.idUsuario = User.Identity.GetUserId();
                db.Presupuestos.Add(presupuestos);
                 db.SaveChanges();
                int id = presupuestos.idPresupuesto;
                //oculto el modal y redirecciono a la pagina para agregar los servicios
                string cont = "<script>  $('#myModal').modal('hide'); window.location.href = '/Presupuestos/Edit/" + id.ToString() + "';  </script>";
                return Content(cont);
                // return RedirectToAction("Edit", new { id = presupuestos.idPresupuesto  });
                //return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", presupuestos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Presupuestado)), "idEvento", "descripcion", presupuestos.idEvento);
            return PartialView(presupuestos);
        }

        // GET: Presupuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
            if (presupuestos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", presupuestos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.idEvento!=0 && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Presupuestado)), "idEvento", "descripcion", presupuestos.idEvento);
            return PartialView(presupuestos);
        }

        // POST: Presupuestos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPresupuesto,activo,idEvento,descripcion,observaciones,fechaInicio,fechaFin,idUsuario,estado, probabilidad, bonificacion")] Presupuestos presupuestos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presupuestos).State = EntityState.Modified;
                presupuestos.activo=true;
                presupuestos.editadoPor = User.Identity.GetUserId();
                presupuestos.fechaEditado = DateTime.Now;
                 db.SaveChanges();      
              
              return  RedirectToAction("Index",new { id =presupuestos.idEvento });
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", presupuestos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.idEvento != 0 && (p.estado==estadoEvento.Aceptado || p.estado ==estadoEvento.Presupuestado)), "idEvento", "descripcion", presupuestos.idEvento);
            return PartialView(presupuestos);
        }

        // GET: Presupuestos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
            if (presupuestos == null)
            {
                return HttpNotFound();
            }
            return PartialView(presupuestos);
        }

        // POST: Presupuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
             presupuestos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Presupuestos/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
            if (presupuestos == null)
            {
                return HttpNotFound();
            }
            return PartialView(presupuestos);
        }

        // POST: Presupuestos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Presupuestos presupuestos =  db.Presupuestos.Find(id);
            presupuestos.activo=true;
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
