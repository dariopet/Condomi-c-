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
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class Pagos2Controller : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();




        // GET: Pagos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index(int? id)
        {
            var pagos = db.Pagos.Include(p => p.AspNetUsers).Include(p => p.Cuentas).Include(p => p.Egresos);
            if (id == null)
            {
                ViewBag.unEgreso = "no";
                return View( pagos.OrderByDescending(a => a.fecha).ToList());
            }
            else
            {
                ViewBag.unEgreso = "si";
                ViewBag.descripcion = db.Egresos.Find(id).descripcion;
                ViewBag.idEgreso = id;
                return View( pagos.Where(a => a.idEgreso == id).OrderByDescending(a => a.fecha).ToList());

            }
        }

        public JsonResult getPagado(int idTipoEgreso, String fechaPago, int idTercero)
        {
            DateTime fecha = Convert.ToDateTime(fechaPago);// DateTime.Now;           
            decimal totalEgresado;
            //decimal retencion;
            //retencion=db.DetalleEgresos.Where(a => a.idTipoEgreso == idTipoEgreso && a.Egresos.fechaVencimiento.Year == fecha.Year && a.Egresos.fechaVencimiento.Month == fecha.Month).Select(n=> new {  n.Sum(n => n.monto) }).First();

            var data = db.Pagos.Where(a => a.Egresos.DetalleEgresos.FirstOrDefault().idTipoEgreso == idTipoEgreso && a.fecha.Year == fecha.Year && a.fecha.Month == fecha.Month && a.Egresos.idTercero==idTercero);
            if(data.Count()!=0)
            {
            totalEgresado = data.Sum(d => d.monto);

            }
            else
            {
                totalEgresado = 0;
            }
          
            return new JsonResult { Data = JsonConvert.SerializeObject(totalEgresado), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        // GET: Pagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagos pagos =  db.Pagos.Find(id);
            if (pagos == null)
            {
                return HttpNotFound();
            }
            return PartialView(pagos);
        }

        // GET: Pagos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create(int idEgreso)
        {
            Pagos pago = new Pagos();
            pago.idEgreso = idEgreso;
            pago.Egresos  = db.Egresos.Find(idEgreso);
            Egresos egreso = db.Egresos.Find(idEgreso);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");

            if (tieneAdelantos(pago.Egresos.idTercero))
            {  ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion");            }
            else
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta!=3), "idCuenta", "descripcion");  }
                       
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion");
            ViewBag.idAdelanto = new SelectList(db.Adelantos.Where(p=>p.idTercero== egreso.idTercero ) , "idAdelanto", "descripcion");
            //no haya sido asignado a ningun cobro aun o que fueron utilizados por el mismo tercero
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Pagos.Count() == 0 || p.Pagos.Where(a => a.Egresos.idTercero == egreso.idTercero).Count() > 0) ), "idCheque", "numCheque");
          
         
            return PartialView(pago);
           
        }

        // POST: Pagos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPago,idEgreso,fecha,monto,idCuenta,idUsuario, retenido, idAdelanto, idcheque")] Pagos pagos)
        {
            if (ModelState.IsValid)
            {
                pagos.idUsuario = User.Identity.GetUserId();
                
                Cuentas cuenta = db.Cuentas.Find(pagos.idCuenta);
                cuenta.fondos -= pagos.monto;

                //si no es cheque ni adelanto los pongo en null
                if (pagos.idCuenta != 2)
                {
                    pagos.idCheque = null;
                }
                if (pagos.idCuenta != 3)
                {
                    pagos.idAdelanto = null;
                }

                db.Pagos.Add(pagos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }
            pagos.Egresos = db.Egresos.Find(pagos.idEgreso);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", pagos.idUsuario);

            if (tieneAdelantos(pagos.Egresos.idTercero))
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", pagos.idCuenta); }
            else
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 3), "idCuenta", "descripcion", pagos.idCuenta); }
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", pagos.idEgreso);
            ViewBag.idAdelanto = new SelectList(db.Adelantos.Where(p => p.idTercero == pagos.Egresos.idTercero), "idAdelanto", "descripcion", pagos.idAdelanto);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Pagos.Count() == 0 || p.Pagos.Where(a => a.Egresos.idTercero == pagos.Egresos.idTercero).Count() > 0) ), "idCheque", "numCheque", pagos.idCheque);

            return PartialView(pagos);
        }

        // GET: Pagos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagos pagos =  db.Pagos.Find(id);
            if (pagos == null)
            {
                return HttpNotFound();
            }

            //guardo los datos originales temporalmente para tener acceso despues
            TempData["cuenta"] = pagos.idCuenta;
            TempData["monto"] = pagos.monto;
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", pagos.idUsuario);
            if (tieneAdelantos(pagos.Egresos.idTercero))
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", pagos.idCuenta); }
            else
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 3), "idCuenta", "descripcion", pagos.idCuenta); }
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", pagos.idEgreso);
            ViewBag.idAdelanto = new SelectList(db.Adelantos.Where(p => p.idTercero == pagos.Egresos.idTercero), "idAdelanto", "descripcion", pagos.idAdelanto);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Pagos.Count() == 0 || p.Pagos.Where(a => a.Egresos.idTercero == pagos.Egresos.idTercero).Count() > 0) ), "idCheque", "numCheque", pagos.idCheque);

            return PartialView(pagos);
        }

        // POST: Pagos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPago,idEgreso,fecha,monto,idCuenta,idUsuario, retenido, idAdelanto, idcheque")] Pagos pagos)
        {
            if (ModelState.IsValid)
            {
                //si no es cheque ni adelanto los pongo en null
                if (pagos.idCuenta != 2)
                {
                    pagos.idCheque = null;
                }
                if (pagos.idCuenta != 3)
                {
                    pagos.idAdelanto = null;
                }
                //descuento el monto original de la cuenta original
                //busco la cuenta nueva y lo sumo
                Cuentas cuenta1 = db.Cuentas.Find(TempData["cuenta"]);
                cuenta1.fondos += Convert.ToDecimal(TempData["monto"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (pagos.idCuenta != Convert.ToInt32(TempData["cuenta"]))
                {
                    Cuentas cuenta2 = db.Cuentas.Find(pagos.idCuenta);
                    cuenta2.fondos -= pagos.monto;
                }
                else
                {
                    cuenta1.fondos -= pagos.monto;
                }
                db.Entry(pagos).State = EntityState.Modified;
                pagos.editadoPor = User.Identity.GetUserId();
                pagos.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", pagos.idUsuario);
            if (tieneAdelantos(pagos.Egresos.idTercero))
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0), "idCuenta", "descripcion", pagos.idCuenta); }
            else
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta != 3), "idCuenta", "descripcion", pagos.idCuenta); }
            ViewBag.idEgreso = new SelectList(db.Egresos.Where(p => p.activo == true), "idEgreso", "descripcion", pagos.idEgreso);
            ViewBag.idAdelanto = new SelectList(db.Adelantos.Where(p => p.idTercero == pagos.Egresos.idTercero), "idAdelanto", "descripcion", pagos.idAdelanto);
            ViewBag.idCheque = new SelectList(db.Cheques.Where(p => p.activo == true && (p.Pagos.Count() == 0 || p.Pagos.Where(a => a.Egresos.idTercero == pagos.Egresos.idTercero).Count() > 0) ), "idCheque", "numCheque", pagos.idCheque);

            return PartialView(pagos);
        }

        // GET: Pagos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagos pagos =  db.Pagos.Find(id);
            if (pagos == null)
            {
                return HttpNotFound();
            }
            return PartialView(pagos);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pagos pagos =  db.Pagos.Find(id);
            db.Pagos.Remove(pagos);
            Cuentas cuenta=db.Cuentas.Find(pagos.idCuenta);
            cuenta.fondos += pagos.monto;
                  
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Pagos/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagos pagos =  db.Pagos.Find(id);
            if (pagos == null)
            {
                return HttpNotFound();
            }
            return PartialView(pagos);
        }

        // POST: Pagos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Pagos pagos =  db.Pagos.Find(id);         
             db.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult getDatosAdelanto(int idAdelanto)
        {
            decimal montoDisp = 0;
            decimal yaUtilizado = 0;
            Adelantos adelanto = db.Adelantos.Find(idAdelanto);
            var data4 = db.Pagos.Where(a => a.idAdelanto == idAdelanto);
            if (data4.Count() != 0)
            {
                yaUtilizado = data4.Sum(d => d.monto);
            }
            else
            {
                yaUtilizado = 0;
            }
            montoDisp = adelanto.monto - yaUtilizado;

            return Json(new { montoDisponible = montoDisp, descripcion = adelanto.descripcion, fecha=adelanto.fecha }, JsonRequestBehavior.AllowGet);
        }

        public Decimal saldoAdelanto(int idAdelanto)
        {
            decimal montoDisp = 0;
            decimal yaUtilizado = 0;
            Adelantos adelanto = db.Adelantos.Find(idAdelanto);
            var data4 = db.Pagos.Where(a => a.idAdelanto == idAdelanto);
            if (data4.Count() != 0)
            {
                yaUtilizado = data4.Sum(d => d.monto);
            }
            else
            {
                yaUtilizado = 0;
            }
            montoDisp = adelanto.monto - yaUtilizado;

            return montoDisp;
        }

        public JsonResult getDatosCheque(int idCheque)
        {
            decimal montoDisp = 0;
            decimal yaUtilizado = 0;
            Cheques cheque = db.Cheques.Find(idCheque);
            var data4 = db.Pagos.Where(a => a.idCheque == idCheque);
            if (data4.Count() != 0)
            {
                yaUtilizado = data4.Sum(d => d.monto);
            }
            else
            {
                yaUtilizado = 0;
            }
            montoDisp = cheque.monto - yaUtilizado;

            return Json(new { montoDisponible = montoDisp, banco = cheque.banco, recibido = cheque.Terceros.nombre }, JsonRequestBehavior.AllowGet);
        }


        private Boolean tieneAdelantos(int idTercero)
        {
            List<Adelantos> adelantos = new List<Adelantos>();
            adelantos = db.Adelantos.ToList();
            for(int i= adelantos.Count -1; i>=0; i--)
            {
                if (adelantos.ElementAt(i).idTercero!=idTercero || saldoAdelanto(adelantos.ElementAt(i).idAdelanto) == 0)
                {
                    adelantos.RemoveAt(i);
                }
            }
            
            if (adelantos.Count ==0  )
            {  return false;    }
            else { return true; }
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
