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
    
        public class CobrosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Cobros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index(int? id)
        {
            var cobros = db.Cobros.Include(c => c.AspNetUsers).Include(c => c.Cheques).Include(c => c.Cuentas).Include(c => c.Ingresos);
            if (id == null)
            {
                ViewBag.unIngreso = "no";
                TempData["idIngreso"] = null;
                //return View( cobros.OrderByDescending(a => a.fecha).ToList());
            }
            else
            {
                TempData["idIngreso"] = id;
                ViewBag.unIngreso = "si";
                ViewBag.descripcion = db.Ingresos.Find(id).descripcion;
                ViewBag.idIngreso = id;
                //return View( cobros.Where(a=>a.idIngreso== id).OrderByDescending(a => a.fecha).ToList());
                
            }
            return View();      
        }

        

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var cobros = db.Cobros;
            if (TempData["idIngreso"] == null)
            {
                return PartialView(cobros.Where(a => a.fecha >= inicio && a.fecha <= fin).Include(p => p.AspNetUsers).Include(p => p.Cuentas).Include(p => p.Ingresos).OrderByDescending(a => a.fecha));
            }
            else
            {
                int idIngreso = Convert.ToInt32(TempData["idIngreso"]);
                return PartialView(cobros.Where(a => a.fecha >= inicio && a.fecha <= fin && a.idIngreso == idIngreso).Include(p => p.AspNetUsers).Include(p => p.Cuentas).Include(p => p.Ingresos).OrderByDescending(a => a.fecha));

            }

        }



        // GET: Cobros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobros cobros =  db.Cobros.Find(id);
            if (cobros == null)
            {
                return HttpNotFound();
            }
            return PartialView(cobros);
        }

        // GET: Cobros/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create(int idIngreso)
        {
            Cobros cobro = new Cobros();
            cobro.idIngreso = idIngreso;
            Ingresos ingreso = db.Ingresos.Find(idIngreso);              
            
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            //la cuenta no tiene q ser "ninguna" ni "vales"
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta!=1), "idCuenta", "descripcion");
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion");
            //no haya sido asignado a ningun cobro aun o que fueron utilizados por el mismo tercero
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Cobros.Count() == 0 || p.Cobros.Where(a => a.Ingresos.idTercero == ingreso.idTercero).Count() > 0) && p.idTercero == ingreso.idTercero), "idCheque", "numCheque");

            return PartialView(cobro);
        }

        // POST: Cobros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCobro,idIngreso,fecha,monto,idCuenta,idUsuario,fechaEditado,idCheque")] Cobros cobros)
        {
            if (ModelState.IsValid)
            {
                if(cobros.idCuenta!=2)
                {
                    cobros.idCheque = null;
                }
                cobros.idUsuario = User.Identity.GetUserId();               
                Cuentas cuenta = db.Cuentas.Find(cobros.idCuenta);
                cuenta.fondos  += cobros.monto;
                db.Cobros.Add(cobros);
                
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            cobros.Ingresos = db.Ingresos.Find(cobros.idIngreso);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Cobros.Count() == 0 || p.Cobros.Where(a => a.Ingresos.idTercero == cobros.Ingresos.idTercero).Count() > 0) && p.idTercero == cobros.Ingresos.idTercero), "idCheque", "numCheque", cobros.idCheque);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cobros.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 1), "idCuenta", "descripcion", cobros.idCuenta);
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", cobros.idIngreso);
            return PartialView(cobros);
        }

        // GET: Cobros/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobros cobros =  db.Cobros.Find(id);
            if (cobros == null)
            {
                return HttpNotFound();
            }
            TempData["cuenta"] = cobros.idCuenta;
            TempData["monto"] = cobros.monto;
            cobros.Ingresos = db.Ingresos.Find(cobros.idIngreso);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cobros.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 1), "idCuenta", "descripcion", cobros.idCuenta);
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", cobros.idIngreso);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Cobros.Count() == 0 || p.Cobros.Where(a => a.Ingresos.idTercero == cobros.Ingresos.idTercero).Count() > 0) && p.idTercero == cobros.Ingresos.idTercero), "idCheque", "numCheque", cobros.idCheque);

            return PartialView(cobros);
        }

        // POST: Cobros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCobro,idIngreso,fecha,monto,idCuenta,idUsuario, idCheque")] Cobros cobros)
        {
            if (ModelState.IsValid)
            {
                //si era un cheque y ahora no, lo pongo como null
                if (cobros.idCuenta != 2)
                {
                    cobros.idCheque = null;
                }
                //descuento el monto original de la cuenta original
                //busco la cuenta nueva y lo sumo
                Cuentas cuenta1 = db.Cuentas.Find(TempData["cuenta"] );
                    cuenta1.fondos -= Convert.ToDecimal( TempData["monto"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (cobros.idCuenta != Convert.ToInt32(TempData["cuenta"]))
                {
                    Cuentas cuenta2 = db.Cuentas.Find(cobros.idCuenta);
                    cuenta2.fondos += cobros.monto;
                }
                else
                {
                    cuenta1.fondos += cobros.monto;
                }

                cobros .editadoPor = User.Identity.GetUserId();
                cobros.fechaEditado = DateTime.Now;
                db.Entry(cobros).State = EntityState.Modified;                
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            cobros.Ingresos = db.Ingresos.Find(cobros.idIngreso);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", cobros.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 1), "idCuenta", "descripcion", cobros.idCuenta);
            ViewBag.idIngreso = new SelectList(db.Ingresos.Where(p => p.activo == true), "idIngreso", "descripcion", cobros.idIngreso);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Cobros.Count() == 0 || p.Cobros.Where(a => a.Ingresos.idTercero == cobros.Ingresos.idTercero).Count() > 0) && p.idTercero == cobros.Ingresos.idTercero), "idCheque", "numCheque", cobros.idCheque);

            return PartialView(cobros);
        }

        // GET: Cobros/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobros cobros =  db.Cobros.Find(id);
            if (cobros == null)
            {
                return HttpNotFound();
            }
            return PartialView(cobros);
        }

        // POST: Cobros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cobros cobros =  db.Cobros.Find(id);
            Cuentas cuenta = db.Cuentas.Find(cobros.idCuenta);
            cuenta.fondos -= cobros.monto;
            db.Cobros.Remove(cobros);          
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Cobros/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobros cobros =  db.Cobros.Find(id);
            if (cobros == null)
            {
                return HttpNotFound();
            }
            return PartialView(cobros);
        }

        // POST: Cobros/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Cobros cobros =  db.Cobros.Find(id);           
             db.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult getDatosCheque(int idCheque)
        {
            decimal montoDisp = 0;
            decimal yaUtilizado = 0;
            Cheques cheque = db.Cheques.Find(idCheque);
            var data4 = db.Cobros.Where(a => a.idCheque==idCheque);
            if (data4.Count() != 0)
            {
                yaUtilizado = data4.Sum(d => d.monto);
            }
            else
            {
                yaUtilizado = 0;
            }
            montoDisp = cheque.monto - yaUtilizado;            

            return Json(new { montoDisponible= montoDisp, banco=cheque.banco, recibido=cheque.Terceros.nombre   }, JsonRequestBehavior.AllowGet);
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
