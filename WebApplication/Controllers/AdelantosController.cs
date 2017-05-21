using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class AdelantosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Adelantos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var adelantos = db.Adelantos.Include(a => a.AspNetUsers).Include(a => a.Editado).Include(a => a.Terceros);

            return PartialView(adelantos.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));
        }


        // GET: Adelantos/Details/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adelantos adelantos = db.Adelantos.Find(id);
            if (adelantos == null)
            {
                return HttpNotFound();
            }
            return PartialView(adelantos);
        }

        // GET: Adelantos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");                                             
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            //empleado o tecnico   o chofer (14)                                          
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero==1 || p.idTipoTercero== 2 || p.idTipoTercero == 14) ), "idTercero", "nombre").OrderBy (p=>p.Text );
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion");

            return PartialView();
        }

        // POST: Adelantos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idAdelanto,descripcion,monto,fecha,fechaEditado,idUsuario,editadoPor,idTercero,idcuenta")] Adelantos adelantos)
        {
            if (ModelState.IsValid)
            {
                adelantos.idUsuario = User.Identity.GetUserId();
                adelantos.fecha = DateTime.Now;
                Cuentas cuenta = db.Cuentas.Find(adelantos.idCuenta);
                cuenta.fondos -= adelantos.monto;
                db.Adelantos.Add(adelantos);
                db.SaveChanges();
                 return Json(new { success = true });
                
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", adelantos.idCuenta);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.editadoPor);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2 || p.idTipoTercero == 14)), "idTercero", "nombre", adelantos.idTercero).OrderBy(p => p.Text);
            return PartialView(adelantos);
        }

        // GET: Adelantos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adelantos adelantos = db.Adelantos.Find(id);
            if (adelantos == null)
            {
                return HttpNotFound();
            }
            //guardo los datos originales temporalmente para tener acceso despues
            TempData["cuenta"] = adelantos.idCuenta;
            TempData["monto"] = adelantos.monto;
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", adelantos.idCuenta);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.editadoPor);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2 || p.idTipoTercero == 14)), "idTercero", "nombre", adelantos.idTercero).OrderBy(p => p.Text);
            return PartialView(adelantos);
        }

        // POST: Adelantos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idAdelanto,descripcion,monto,fecha,fechaEditado,idUsuario,editadoPor,idTercero,idcuenta")] Adelantos adelantos)
        {
            if (ModelState.IsValid)
            {
                //descuento el monto original de la cuenta original
                //busco la cuenta nueva y lo sumo
                Cuentas cuenta1 = db.Cuentas.Find(TempData["cuenta"]);
                cuenta1.fondos += Convert.ToDecimal(TempData["monto"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (adelantos.idCuenta != Convert.ToInt32(TempData["cuenta"]))
                {
                    Cuentas cuenta2 = db.Cuentas.Find(adelantos.idCuenta);
                    cuenta2.fondos -= adelantos.monto;
                }
                else
                {
                    cuenta1.fondos -= adelantos.monto;
                }

                adelantos.editadoPor = User.Identity.GetUserId();
                adelantos.fechaEditado = DateTime.Now;
                db.Entry(adelantos).State = EntityState.Modified;              
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", adelantos.idCuenta);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", adelantos.editadoPor);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2 || p.idTipoTercero == 14)), "idTercero", "nombre", adelantos.idTercero).OrderBy(p => p.Text);
            return PartialView(adelantos);
        }

        // GET: Adelantos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adelantos adelantos = db.Adelantos.Find(id);
            if (adelantos == null)
            {
                return HttpNotFound();
            }
            return PartialView(adelantos);
        }

        // POST: Adelantos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adelantos adelantos = db.Adelantos.Find(id);
           
            db.SaveChanges();
            return Json(new { success = true });
        }


        //devuelve el total pagado de un adelanto
        public String getPagadoAdelanto(int idAdelanto)
        {
            Adelantos adelanto = db.Adelantos.Find(idAdelanto);
            decimal totalPagado = 0;

            foreach (var det in adelanto.Pagos.Where(a => a.idAdelanto == adelanto.idAdelanto))
            {
                totalPagado += det.monto;
            }
            return totalPagado.ToString();
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
