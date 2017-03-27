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
    public class TransferenciasController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Transferencias
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var transferencias = db.Transferencias.Include(t => t.AspNetUsers).Include(t => t.Cuentas).Include(t => t.Cuentas1);

            return PartialView(transferencias.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));

        }


        // GET: Transferencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transferencias transferencias =  db.Transferencias.Find(id);
            if (transferencias == null)
            {
                return HttpNotFound();
            }
            return PartialView(transferencias);
        }

        // GET: Transferencias/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
                                             
            ViewBag.idCuentaOrigen = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta!=0), "idCuenta", "descripcion");                                             
            ViewBag.idCuentaDestino = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0), "idCuenta", "descripcion");
                                             
            return PartialView();
        }

        // POST: Transferencias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTransferencia,idCuentaOrigen,idCuentaDestino,fecha,monto,descripcion")] Transferencias transferencias)
        {
            if (ModelState.IsValid)
            {
               transferencias.idUsuario = User.Identity.GetUserId();
                transferencias.fecha = DateTime.Now;
                db.Transferencias.Add(transferencias);
                Cuentas cuentaO = db.Cuentas.Find(transferencias.idCuentaOrigen);
                cuentaO.fondos -= transferencias.monto;
                Cuentas cuentaD = db.Cuentas.Find(transferencias.idCuentaDestino);
                cuentaD.fondos += transferencias.monto;
                
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", transferencias.idUsuario);
            ViewBag.idCuentaOrigen = new SelectList(db.Cuentas.Where(p => p.idTercero  == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaOrigen);
            ViewBag.idCuentaDestino = new SelectList(db.Cuentas.Where(p => p.idTercero  == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaDestino);
            return PartialView(transferencias);
        }

        // GET: Transferencias/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transferencias transferencias =  db.Transferencias.Find(id);
            if (transferencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", transferencias.idUsuario);
            ViewBag.idCuentaOrigen = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaOrigen);
            ViewBag.idCuentaDestino = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaDestino);
            return PartialView(transferencias);
        }

        // POST: Transferencias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTransferencia,idCuentaOrigen,idCuentaDestino,fecha,monto,descripcion,idUsuario")] Transferencias transferencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transferencias).State = EntityState.Modified;
                transferencias.editadoPor = User.Identity.GetUserId();
                transferencias.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", transferencias.idUsuario);
            ViewBag.idCuentaOrigen = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaOrigen);
            ViewBag.idCuentaDestino = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0), "idCuenta", "descripcion", transferencias.idCuentaDestino);
            return PartialView(transferencias);
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
