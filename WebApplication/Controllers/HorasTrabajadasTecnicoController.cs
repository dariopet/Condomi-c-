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
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class HorasTrabajadasTecnicoController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: HorasTrabajadasTecnico
        public ActionResult Index()
        {
            return View( );
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var horasTrabajadasTecnico = db.HorasTrabajadasTecnico.Include(h => h.AspNetUsers).Include(h => h.Terceros).Include(h => h.Eventos);

            return PartialView(horasTrabajadasTecnico.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));
        }


        // GET: HorasTrabajadasTecnico
        public ActionResult Liquidacion()
        {
            return View( );
        }

        public ActionResult LiquidacionEntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var horasTrabajadasTecnico = db.HorasTrabajadasTecnico.Include(h => h.AspNetUsers).Include(h => h.Terceros).Include(h => h.Eventos);

            return PartialView(horasTrabajadasTecnico.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));
        }

        public JsonResult getPrecioUltimaJornada(int idTercero)
        {           
            decimal ultimoPago;
            decimal dimmer, seguidor;
            var data = db.HorasTrabajadasTecnico.Where(a => a.idTercero == idTercero).OrderByDescending(a=>a.fecha).FirstOrDefault();
            if (data != null)
            {
                ultimoPago = data.precioJornada;
                dimmer = data.dimmer;
                seguidor = data.seguidor;
            }
            else
            {
                dimmer = 0;
                seguidor = 0;
                ultimoPago = 0;
            }
            return Json(new { precioDimmer=dimmer, precioSeguidor=seguidor, precioUltimoPago=ultimoPago },JsonRequestBehavior.AllowGet);           
        }


        // GET: HorasTrabajadasTecnico/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            if (horasTrabajadasTecnico == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasTrabajadasTecnico);
        }

        // GET: HorasTrabajadasTecnico/Create
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre");
                                             
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.estado== estadoEvento.Aceptado), "idEvento", "descripcion");
                                             
            return PartialView();
        }

        // POST: HorasTrabajadasTecnico/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idHorasTrabajadasTecnico,idTercero,fecha,cantidad,idUsuario,idEvento,pagadas,editadoPor,fechaEditado,seguidor,dimmer,precioJornada")] HorasTrabajadasTecnico horasTrabajadasTecnico)
        {
            if (ModelState.IsValid)
            {
                horasTrabajadasTecnico.idUsuario = User.Identity.GetUserId();
                db.HorasTrabajadasTecnico.Add(horasTrabajadasTecnico);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasTrabajadasTecnico.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasTrabajadasTecnico.idTercero);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.estado == estadoEvento.Aceptado), "idEvento", "descripcion", horasTrabajadasTecnico.idEvento);
            return PartialView(horasTrabajadasTecnico);
        }

        // GET: HorasTrabajadasTecnico/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            if (horasTrabajadasTecnico == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasTrabajadasTecnico.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasTrabajadasTecnico.idTercero);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.estado == estadoEvento.Aceptado), "idEvento", "descripcion", horasTrabajadasTecnico.idEvento);
            return PartialView(horasTrabajadasTecnico);
        }

        // POST: HorasTrabajadasTecnico/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idHorasTrabajadasTecnico,idTercero,fecha,cantidad,idUsuario,idEvento,pagadas,editadoPor,fechaEditado,seguidor,dimmer,precioJornada")] HorasTrabajadasTecnico horasTrabajadasTecnico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horasTrabajadasTecnico).State = EntityState.Modified;
                horasTrabajadasTecnico.editadoPor = User.Identity.GetUserId();
                horasTrabajadasTecnico.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", horasTrabajadasTecnico.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", horasTrabajadasTecnico.idTercero);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.estado == estadoEvento.Aceptado), "idEvento", "descripcion", horasTrabajadasTecnico.idEvento);
            return PartialView(horasTrabajadasTecnico);
        }

        // GET: HorasTrabajadasTecnico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            if (horasTrabajadasTecnico == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasTrabajadasTecnico);
        }

        // POST: HorasTrabajadasTecnico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            db.HorasTrabajadasTecnico.Remove(horasTrabajadasTecnico);
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: HorasTrabajadasTecnico/Delete/5
        public ActionResult Pagadas(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            if (horasTrabajadasTecnico == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasTrabajadasTecnico);
        }

        // POST: HorasTrabajadasTecnico/Delete/5
        [HttpPost, ActionName("Pagadas")]
        [ValidateAntiForgeryToken]
        public ActionResult PagadasConfirmed(int id)
        {
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            horasTrabajadasTecnico.pagadas = true;
            horasTrabajadasTecnico.pagadoPor= User.Identity.GetUserId();
            horasTrabajadasTecnico.fechaPago = DateTime.Now;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: HorasTrabajadasTecnico/Delete/5
        public ActionResult NoPagadas(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            if (horasTrabajadasTecnico == null)
            {
                return HttpNotFound();
            }
            return PartialView(horasTrabajadasTecnico);
        }

        // POST: HorasTrabajadasTecnico/Delete/5
        [HttpPost, ActionName("NoPagadas")]
        [ValidateAntiForgeryToken]
        public ActionResult NoPagadasConfirmed(int id)
        {
            HorasTrabajadasTecnico horasTrabajadasTecnico =  db.HorasTrabajadasTecnico.Find(id);
            horasTrabajadasTecnico.pagadas = false;
            horasTrabajadasTecnico.pagadoPor = User.Identity.GetUserId();
            horasTrabajadasTecnico.fechaPago = DateTime.Now;
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
