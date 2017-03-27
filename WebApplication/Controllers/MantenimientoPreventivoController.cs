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
    public class MantenimientoPreventivoController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: MantenimientoPreventivo
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Index()
        {
                  
            var mantenimientoPreventivo = db.MantenimientoPreventivo.Include(m => m.AspNetUsers).Include(m => m.Editado);
            return View( mantenimientoPreventivo.OrderByDescending(a => a.fechaLimite).ToList());
        }

        // GET: MantenimientoPreventivo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
            if (mantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(mantenimientoPreventivo);
        }

        // GET: MantenimientoPreventivo/Create
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
                                             
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion");
            return PartialView();
        }

        // POST: MantenimientoPreventivo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idMantenimientoPreventivo,descripcion,idUsuario,editadoPor,fechaEditado,fechaGenerado,fechaLimite,prioridad, idModelo")] MantenimientoPreventivo mantenimientoPreventivo)
        {
            if (ModelState.IsValid)
            {
                 mantenimientoPreventivo.activo=true;
               mantenimientoPreventivo.idUsuario= User.Identity.GetUserId();
                mantenimientoPreventivo.fechaGenerado = DateTime.Now;
                db.MantenimientoPreventivo.Add(mantenimientoPreventivo);
                db.SaveChanges();
                int id = mantenimientoPreventivo.idMantenimientoPreventivo;
                //agrego todos los equipos del grupo seleccionado como pendientes
                var data = db.Equipos.Where(p => p.idModelo == mantenimientoPreventivo.idModelo && p.activo==true);
                foreach (var item in data)
                {
                    DetalleMantenimientoPreventivo detalle = new DetalleMantenimientoPreventivo();
                    detalle.idEquipo = item.idEquipo;
                    detalle.idMantenimientoPreventivo = mantenimientoPreventivo.idMantenimientoPreventivo;
                    detalle.estado = estadoMantenimientoPreventivo.Pendiente;
                    db.DetalleMantenimientoPreventivo.Add(detalle);
                }
                
                db.SaveChanges();
                //oculto el modal y redirecciono a la pagina para agregar los servicios
                string cont = "<script>  $('#myModal').modal('hide'); window.location.href = '/MantenimientoPreventivo/Edit/" + id.ToString() + "';  </script>";
                return Content(cont);

            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.editadoPor);
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", mantenimientoPreventivo.idModelo);
            return PartialView(mantenimientoPreventivo);
        }

        // GET: MantenimientoPreventivo/Edit/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
            if (mantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.editadoPor);
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", mantenimientoPreventivo.idModelo);
            return PartialView(mantenimientoPreventivo);
        }

        // POST: MantenimientoPreventivo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMantenimientoPreventivo,descripcion,idUsuario,editadoPor,fechaEditado,fechaGenerado,fechaLimite,prioridad, idModelo")] MantenimientoPreventivo mantenimientoPreventivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mantenimientoPreventivo).State = EntityState.Modified;
                mantenimientoPreventivo.activo=true;
                mantenimientoPreventivo.editadoPor = User.Identity.GetUserId();
                mantenimientoPreventivo.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", mantenimientoPreventivo.editadoPor);
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", mantenimientoPreventivo.idModelo);
            return PartialView(mantenimientoPreventivo);
        }

        // GET: MantenimientoPreventivo/Delete/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
            if (mantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(mantenimientoPreventivo);
        }

        // POST: MantenimientoPreventivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
             mantenimientoPreventivo.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: MantenimientoPreventivo/Activate/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
            if (mantenimientoPreventivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(mantenimientoPreventivo);
        }

        // POST: MantenimientoPreventivo/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            MantenimientoPreventivo mantenimientoPreventivo =  db.MantenimientoPreventivo.Find(id);
            mantenimientoPreventivo.activo=true;
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
