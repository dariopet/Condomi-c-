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
    public class ChequesController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Cheques
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            var cheques = db.Cheques.Include(c => c.AspNetUsers).Include(c => c.Terceros);
            return View( cheques.OrderByDescending(a => a.fechaCobro).ToList());
        }

        // GET: Cheques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheques cheques =  db.Cheques.Find(id);
            if (cheques == null)
            {
                return HttpNotFound();
            }
            return PartialView(cheques);
        }

        // GET: Cheques/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero!=0), "idTercero", "nombre");                                             
            return PartialView();
        }

        // POST: Cheques/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCheque,activo,banco,fechaEmitido,fechaCobro,cobrado,idTercero,monto,numCheque,idUsuario,enBlanco")] Cheques cheques)
        {
            if (ModelState.IsValid)
            {
                cheques.idUsuario= User.Identity.GetUserId();             
                cheques.activo=true;
                db.Cheques.Add(cheques);
                 db.SaveChanges();
                return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cheques.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", cheques.idTercero);
            return PartialView(cheques);
        }

        // GET: Cheques/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheques cheques =  db.Cheques.Find(id);
            if (cheques == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cheques.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", cheques.idTercero);
            return PartialView(cheques);
        }

        // POST: Cheques/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCheque,activo,banco,fechaEmitido,fechaCobro,cobrado,idTercero,monto,numCheque,idUsuario,enBlanco")] Cheques cheques)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cheques).State = EntityState.Modified;
                cheques.activo=true;
                cheques.editadoPor = User.Identity.GetUserId();
                cheques.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cheques.idUsuario);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", cheques.idTercero);
            return PartialView(cheques);
        }

        // GET: Cheques/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheques cheques =  db.Cheques.Find(id);
            if (cheques == null)
            {
                return HttpNotFound();
            }
            return PartialView(cheques);
        }

        // POST: Cheques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cheques cheques =  db.Cheques.Find(id);
             cheques.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Cheques/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cheques cheques =  db.Cheques.Find(id);
            if (cheques == null)
            {
                return HttpNotFound();
            }
            return PartialView(cheques);
        }

        // POST: Cheques/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Cheques cheques =  db.Cheques.Find(id);
            cheques.activo=true;
             db.SaveChanges();
            return Json(new { success = true });
        }


        //devuelve el total cobrado de un cheque
        public String getCobradoCheque(int idCheque)
        {
            var cobros = db.Cobros;
            decimal totalCobrado = 0;
            foreach (var cob in cobros.Where(a => a.idCheque == idCheque))
            {
                totalCobrado += cob.monto;
            }
            return (totalCobrado.ToString());
        }

        //devuelve el total pagado de un cheque
        public String getPagadoCheque(int idCheque)
        {
            var pagos = db.Pagos;
            decimal totalPagado = 0;
            foreach (var pag in pagos.Where(a => a.idCheque == idCheque))
            {
                totalPagado += pag.monto;
            }
            return (totalPagado.ToString());
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
