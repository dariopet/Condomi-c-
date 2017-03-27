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
    public class ValesController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Vales
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var vales = db.Vales.Include(v => v.AspNetUsers).Include(v => v.Editado).Include(v => v.Cuentas).Include(v => v.Terceros);

            return PartialView(vales.Where(a => a.fechaGenerado >= inicio && a.fechaGenerado <= fin).OrderByDescending(a => a.fechaGenerado));
        }


        // GET: Vales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vales vales = db.Vales.Find(id);
            if (vales == null)
            {
                return HttpNotFound();
            }
            return PartialView(vales);
        }

        // GET: Vales/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");                                            
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");     //q la cuenta de origen sea propia q no sea "ninguna ni vales"                                        
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0 && p.idCuenta!=1), "idCuenta", "descripcion");                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero==1 || p.idTercero==2)), "idTercero", "nombre");
                                             
            return PartialView();
        }

        // POST: Vales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idVale,monto,idCuenta,idUsuario,editadoPor,fechaGenerado,fechaEditado,idTercero,rendido,activo,descripcion, idTransferencia")] Vales vales)
        {
            if (ModelState.IsValid)
            {
                 vales.activo=true;
                vales.fechaGenerado = DateTime.Now;
                //genero una transferencia
                vales.idUsuario= User.Identity.GetUserId();
                Transferencias trans = new Transferencias();
                trans.idUsuario= User.Identity.GetUserId();
                trans.idCuentaDestino = 1;//vales
                trans.idCuentaOrigen = vales.idCuenta;
                trans.fecha = DateTime.Now;
                trans.descripcion = "Tranferencia para el vale(" + vales.descripcion +")";
                trans.monto = vales.monto;
                db.Transferencias.Add(trans);
                //modifico los saldos
                Cuentas cuenta = db.Cuentas.Find(vales.idCuenta);
                cuenta.fondos -= vales.monto;
                Cuentas cuentaVale = db.Cuentas.Find(1);//busco la cuenta de vales
                cuentaVale.fondos += vales.monto;
                db.Vales.Add(vales);
                db.SaveChanges();
                vales.idTransferencia = trans.idTransferencia;
                db.SaveChanges();
                return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.editadoPor);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0 && p.idCuenta != 1), "idCuenta", "descripcion", vales.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTercero == 2)), "idTercero", "nombre", vales.idTercero);
            return PartialView(vales);
        }

        // GET: Vales/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vales vales = db.Vales.Find(id);
            if (vales == null)
            {
                return HttpNotFound();
            }
            //guardo los datos originales temporalmente para tener acceso despues
            TempData["cuenta"] = vales.idCuenta;
            TempData["monto"] = vales.monto;

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.editadoPor);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0 && p.idCuenta != 1), "idCuenta", "descripcion", vales.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTercero == 2)), "idTercero", "nombre", vales.idTercero);
            return PartialView(vales);
        }

        // POST: Vales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idVale,monto,idCuenta,idUsuario,editadoPor,fechaGenerado,fechaEditado,idTercero,rendido,activo,descripcion, idTransferencia")] Vales vales)
        {
            if (ModelState.IsValid)
            {
                //descuento el monto original de la cuenta original
                //busco la cuenta nueva y lo sumo
                Cuentas cuenta1 = db.Cuentas.Find(TempData["cuenta"]);
                cuenta1.fondos += Convert.ToDecimal(TempData["monto"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (vales.idCuenta != Convert.ToInt32( TempData["cuenta"]))
                {
                    Cuentas cuenta2 = db.Cuentas.Find(vales.idCuenta);
                    cuenta2.fondos -= vales.monto;
                }
                else
                {
                    cuenta1.fondos -= vales.monto;
                }
                //busco la cuenta vale le resto el monto anterior y le sumo el nuevo
                Cuentas cuentaVale = db.Cuentas.Find(1);
                cuentaVale.fondos -= Convert.ToDecimal(TempData["monto"]);
                cuentaVale.fondos += vales.monto;


                //busco la transferencia original y la edito
                Transferencias trans = db.Transferencias.Find(vales.idTransferencia);
                trans.idCuentaOrigen = vales.idCuenta;
                trans.monto = vales.monto;
                trans.fechaEditado = DateTime.Now;
                trans.editadoPor= User.Identity.GetUserId();
                trans.descripcion = "Tranferencia para el vale(" + vales.descripcion + ")";

                vales.editadoPor= User.Identity.GetUserId();
                vales.fechaEditado = DateTime.Now;
                db.Entry(vales).State = EntityState.Modified;
                vales.activo=true;
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.idUsuario);
            ViewBag.editadoPor = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", vales.editadoPor);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0 && p.idCuenta != 0 && p.idCuenta != 1), "idCuenta", "descripcion", vales.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTercero == 2)), "idTercero", "nombre", vales.idTercero);
            return PartialView(vales);
        }

        // GET: vales/rendido/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Rendido(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vales vales = db.Vales.Find(id);
            if (vales == null)
            {
                return HttpNotFound();
            }
            return PartialView(vales);
        }

        // POST: vales/rendido/5
        [HttpPost, ActionName("Rendido")]
        [ValidateAntiForgeryToken]
        public ActionResult RendidoConfirmed(int id)
        {
            Vales vales = db.Vales.Find(id);
            vales.rendido = true;
            vales.editadoPor= User.Identity.GetUserId();
            vales.fechaEditado = DateTime.Now;
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
