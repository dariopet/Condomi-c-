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
using System.Text;
using System.IO;

namespace WebApplication.Controllers
{
    public class PagosController : Controller
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
                TempData["idEgreso"] = null;
                //return View(pagos.OrderByDescending(a => a.fecha).ToList());
            }
            else
            {
                TempData["idEgreso"] = id;
                ViewBag.unEgreso = "si";
                ViewBag.descripcion = db.Egresos.Find(id).descripcion;
                ViewBag.idEgreso = id;
                //return View(pagos.Where(a => a.idEgreso == id).OrderByDescending(a => a.fecha).ToList());

            }
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var pagos = db.Pagos;
            if (TempData["idEgreso"] == null)
            {
                return PartialView(pagos.Where(a => a.fecha >= inicio && a.fecha <= fin).Include(p => p.AspNetUsers).Include(p => p.Cuentas).Include(p => p.Egresos).OrderByDescending(a => a.idPago ));
            }
            else
            {
                int idEgreso = Convert.ToInt32(TempData["idEgreso"]);
                return PartialView (pagos.Where(a => a.fecha >= inicio && a.fecha <= fin && a.idEgreso == idEgreso).Include(p => p.AspNetUsers).Include(p => p.Cuentas).Include(p => p.Egresos).OrderByDescending(a=>a.idPago ));

            }

        }

        public JsonResult getPagado(int idEgreso)
        {
            decimal totalEgresado=0;
            //decimal retencion;
            //retencion=db.DetalleEgresos.Where(a => a.idTipoEgreso == idTipoEgreso && a.Egresos.fechaVencimiento.Year == fecha.Year && a.Egresos.fechaVencimiento.Month == fecha.Month).Select(n=> new {  n.Sum(n => n.monto) }).First();

            var data = db.Pagos.Where(a=>a.Egresos.idEgreso==idEgreso);
            foreach (Pagos pag in data)
            {
                if (pag.Egresos.tipoFactura == tiposFacturas.A)
                {
                    totalEgresado += pag.monto / (1 + (pag.Egresos.iva / 100));
                }
                if (pag.Egresos.tipoFactura == tiposFacturas.B)
                {
                    var iva = ((pag.monto / ((pag.Egresos.iva / 100) + 1)) - pag.monto) * -1;

                    totalEgresado += pag.monto - iva;
                }
            }

            return new JsonResult { Data = JsonConvert.SerializeObject(totalEgresado), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getPagadoTipo(int idTipoEgreso, String fechaPago, int idTercero)
        {
            DateTime fecha = Convert.ToDateTime(fechaPago);// DateTime.Now;           
            decimal totalEgresado=0;
            //decimal retencion;
            //retencion=db.DetalleEgresos.Where(a => a.idTipoEgreso == idTipoEgreso && a.Egresos.fechaVencimiento.Year == fecha.Year && a.Egresos.fechaVencimiento.Month == fecha.Month).Select(n=> new {  n.Sum(n => n.monto) }).First();

            var data = db.Pagos.Where(a => a.Egresos.DetalleEgresos.FirstOrDefault().idTipoEgreso == idTipoEgreso && a.fecha.Year == fecha.Year && a.fecha.Month == fecha.Month && a.Egresos.idTercero == idTercero);

            foreach (Pagos pag in data)
            {
                if (pag.Egresos.tipoFactura == tiposFacturas.A)
                {
                    totalEgresado += pag.monto /  (1+ (pag.Egresos.iva/100));
                }
                if(pag.Egresos.tipoFactura==tiposFacturas.B)
                {
                    var iva = ((pag.monto / ((pag.Egresos.iva / 100) + 1)) - pag.monto) * -1;

                    totalEgresado += pag.monto - iva;
                }
            }
                       
            return new JsonResult { Data = JsonConvert.SerializeObject(totalEgresado), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //
        public JsonResult getRetenidoTipo(int idTipoEgreso, String fechaPago)
        {
            DateTime fecha = Convert.ToDateTime(fechaPago);// DateTime.Now;           
            //decimal totalEgresado = 0;
            decimal retencion=0;
            //retencion=db.DetalleEgresos.Where(a => a.idTipoEgreso == idTipoEgreso && a.Egresos.fechaVencimiento.Year == fecha.Year && a.Egresos.fechaVencimiento.Month == fecha.Month).Select(n=> new {  n.Sum(n => n.monto) }).First();

            var data = db.Pagos.Where(a => a.Egresos.DetalleEgresos.FirstOrDefault().idTipoEgreso == idTipoEgreso && a.fecha.Year == fecha.Year && a.fecha.Month == fecha.Month);

            foreach (Pagos pag in data)
            {
               
                    retencion += Convert.ToDecimal( pag.retenido);       
               
            
            }

            return new JsonResult { Data = JsonConvert.SerializeObject(retencion), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
        public ActionResult Create(int idEgreso)
        {
            Pagos pago = new Pagos();
            pago.idEgreso = idEgreso;
            pago.Egresos  = db.Egresos.Find(idEgreso);
            Egresos egreso = db.Egresos.Find(idEgreso);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");

            if (tieneAdelantos(pago.Egresos.idTercero))
            {  ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0).OrderBy(a => a.descripcion), "idCuenta", "descripcion");            }
            else
            { ViewBag.idCuenta = new SelectList(db.Cuentas.Where(a => a.idCuenta != 0 && a.idCuenta!= 3 ).OrderBy(a=>a.descripcion) , "idCuenta", "descripcion");  }
                       
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
                pagos.monto -= Convert.ToDecimal(pagos.retenido);

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
                pagos.monto += Convert.ToDecimal(pagos.retenido);
                db.SaveChanges();
                // return Json(new { success = true });

                int id = pagos.idEgreso;
                //oculto el modal y redirecciono a la pagina para agregar los servicios
                string cont = "<script>  $('#myModal').modal('hide'); window.location.href = '/Pagos/Index/" + id.ToString() + "';  </script>";
                return Content(cont);

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
            TempData["retenido"] = pagos.retenido;
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
                cuenta1.fondos -= Convert.ToDecimal(TempData["retenido"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (pagos.idCuenta != Convert.ToInt32(TempData["cuenta"]))
                {
                    Cuentas cuenta2 = db.Cuentas.Find(pagos.idCuenta);
                    pagos.monto -= Convert.ToDecimal(pagos.retenido);
                    cuenta2.fondos -= pagos.monto;
                  
                }
                else
                {
                    pagos.monto -= Convert.ToDecimal(pagos.retenido);

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
            pagos.monto -= Convert.ToDecimal(pagos.retenido);

            cuenta.fondos += pagos.monto;
            
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Pagos/Activate/5
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

        public FileStreamResult exportarRetenciones(int idPago)
        {
            //todo: add some data from your database into that string:
            //var texto = "";
            Terceros condomi = new Terceros();
            //traigo los datos de condomi, para extraer el cuit x ejemplo
            condomi = db.Terceros.Find(0);
            Pagos pago = db.Pagos.Find(idPago);
            Terceros retenido = new Terceros();

            //traigo los datos del retenido

            retenido = db.Terceros.Find(pago.Egresos.idTercero);
            StringBuilder texto = new StringBuilder();

            EgresosController egreso = new EgresosController();
            

            //los numeros padleft, los textos padright
            texto.Append("00".PadLeft(2, '0'));//codigo de comprobante
            texto.Append(DateTime.Now.ToString("dd/MM/yyyy").PadRight(10, ' '));// fecha de emision del comprobante
            texto.Append("".PadRight(16, ' '));// numero de comprobante
            texto.Append(egreso.getTotalEgreso(pago.Egresos.idEgreso).PadLeft(16, '0'));// importe del comprobante
            texto.Append("".ToString().PadLeft(3, '0'));// codigo del impuesto
            texto.Append("".ToString().PadLeft(3, '0'));// codigo de regimen
            texto.Append("".ToString().PadLeft(1, '0'));// codigo de operacion
            texto.Append("".ToString().PadLeft(14, '0'));// base de calculo
            texto.Append(pago.Egresos.fechaGenerada.ToString("dd/MM/yyyy").PadRight(10, ' '));// fecha de emision de la retencion
            texto.Append("".ToString().PadLeft(2, '0'));// codigo de condicion
            texto.Append("".ToString().PadLeft(1, '0'));// retencion practicada a sujetos segun
            texto.Append(pago.retenido.ToString().PadLeft(14, '0'));// importe de la retencion
            texto.Append("".PadLeft(6, '0'));// porcentaje de exclusion
            texto.Append(DateTime.Now.ToString("dd/MM/yyyy").PadRight(10, ' '));// fecha de emision de boletin
            texto.Append("".ToString().PadLeft(2, '0'));// tipo de documento retenido
            texto.Append("".ToString().PadRight(20, ' '));// Nº documento retenido
            texto.Append("".ToString().PadLeft(14, '0'));// Nº de certificado original
            texto.Append("".ToString().PadRight(30, ' '));// Denominacion del ordenante
            texto.Append("".ToString().PadLeft(1, '0'));// acrecentamiento
            texto.Append("".ToString().PadRight(11, ' '));// cuit del pais del retenido
            texto.Append("".ToString().PadRight(11, ' '));// cuit del ordenante


            var byteArray = Encoding.ASCII.GetBytes(texto.ToString());
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "retencion del " + pago.Egresos.fechaGenerada.ToShortDateString().Replace("/","-") + " de " +pago.Egresos.Terceros.nombre +".txt");
        }

        public ActionResult CertificadoRetencion(int idPago)
        {
            CertificadoRetencion certificado = new CertificadoRetencion();
            certificado.pago = db.Pagos.Find(idPago);
            certificado.condomi = db.Terceros.Find(0);
            certificado.retenido = db.Terceros.Find(certificado.pago.Egresos.idTercero);

            return View(certificado);
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
