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
    public class EgresosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();


        // GET: Egresos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {           
            
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var egresos = db.Egresos.Include(e => e.AspNetUsers).Include(e => e.Eventos).Include(e => e.Terceros).Include(t => t.PlanCuentas) ;
                      
            return PartialView(egresos.Where(a => a.fechaGenerada >= inicio && a.fechaGenerada <= fin ).OrderByDescending(a => a.fechaGenerada));            
        }

        public ActionResult ExisteFactura(string factura)
        {
            var egresos = db.Egresos.Where(a => a.activo == true && a.factura == factura);
            int con = 0;
            string utilizada = "";
            foreach (Egresos eg in egresos)
            { 
                if(con>0)
                {
                    utilizada += " y en ";
                }
                utilizada += eg.Terceros.nombre + " el " + eg.fechaGenerada.ToShortDateString() + System.Environment.NewLine;             

                con++;
            }
            if(con==0)
            {
                return Content("");
            }
            else
            {               
                return Content("<div class='alert alert-info' role='alert'> Nº de factura utilizada para: " + utilizada + " </div>");
            }
        }


        // GET: Proximos pagos
        public ActionResult ProximosPagos()
        {   
            return View();
        }
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult ProximosPagosPartial(String tope)
        {
            List<Egresos> listaEgresos = new List<Egresos>();
            DateTime topef = Convert.ToDateTime(tope);
            var egreso = db.Egresos.Where(a => a.fechaVencimiento <= topef );
        
            foreach (Egresos egr in egreso)
            {
                if (Convert.ToDecimal(getSaldoEgreso(egr.idEgreso)) >1)
                {
                    listaEgresos.Add(egr);
                }
            }           
            return PartialView(listaEgresos.OrderBy(a => a.fechaVencimiento));
        }

        // GET: Egresos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos =  db.Egresos.Find(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(egresos);
        }

        // GET: Egresos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion");
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");                                             
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "descripcion");                                             
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado==estadoEvento.Aceptado || p.estado==estadoEvento.Cumplido)), "idEvento", "descripcion");                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0) , "idTercero", "nombre").OrderBy (a=>a.Text  ) ;
            Egresos egr = new Egresos();
            egr.tipoFactura = tiposFacturas.A;
            egr.factura = (db.Egresos.OrderByDescending(p=>p.idEgreso).FirstOrDefault().idEgreso + 1).ToString(); 
            return PartialView(egr);
        }

        // POST: Egresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEgreso,activo,descripcion,factura,tipoFactura,fechaVencimiento,fechaGenerada,fechaPago,idUsuario,idCuenta,idTercero,totalPagado,enBlanco,idEvento,impuestosInternos,ingresosBrutos,iva,percepcionIva,idPlanCuentas, conceptosNoGravados")] Egresos egresos)
        {
            if (ModelState.IsValid)
            {                
                egresos.idUsuario = User.Identity.GetUserId();
                 egresos.activo=true;               
                db.Egresos.Add(egresos);
                 db.SaveChanges();
                int id = egresos.idEgreso;
                //oculto el modal y redirecciono a la pagina para agregar los servicios
                string cont = "<script>  $('#myModal').modal('hide'); window.location.href = '/Egresos/Edit/" + id.ToString() + "';  </script>";
                return Content(cont);

            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", egresos.idPlanCuentas);
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", egresos.idUsuario);        
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", egresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", egresos.idTercero).OrderBy (p=>p.Text );
            return PartialView(egresos);
        }

        // GET: Egresos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos =  db.Egresos.Find(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", egresos.idPlanCuentas);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", egresos.idUsuario);      
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", egresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", egresos.idTercero).OrderBy(p => p.Text);
            return PartialView(egresos);
        }

        // POST: Egresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEgreso,activo,descripcion,factura,tipoFactura,fechaVencimiento,fechaGenerada,fechaPago,idUsuario,idCuenta,idTercero,totalPagado,enBlanco,idEvento,impuestosInternos,ingresosBrutos,iva,percepcionIva,idPlanCuentas, conceptosNoGravados")] Egresos egresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(egresos).State = EntityState.Modified;
                egresos.activo=true;
                egresos.editadoPor = User.Identity.GetUserId();
                egresos.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.idPlanCuentas = new SelectList(db.PlanCuentas.Where(p => p.activo == true), "idPlanCuentas", "descripcion", egresos.idPlanCuentas);

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", egresos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && (p.estado == estadoEvento.Aceptado || p.estado == estadoEvento.Cumplido)), "idEvento", "descripcion", egresos.idEvento);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && p.idTercero != 0), "idTercero", "nombre", egresos.idTercero).OrderBy(p => p.Text);
            return PartialView(egresos);
        }

        // GET: Egresos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos =  db.Egresos.Find(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(egresos);
        }

        // POST: Egresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Egresos egresos =  db.Egresos.Find(id);
             egresos.activo=false;
             db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Egresos/Activate/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos =  db.Egresos.Find(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return PartialView(egresos);
        }

        // POST: Egresos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Egresos egresos =  db.Egresos.Find(id);
            egresos.activo=true;
             db.SaveChanges();
            return Json(new { success = true });
        }

        //devuelve el total de un egreso
        public String getTotalEgreso(int idEgreso)
        {
            decimal total = 0;
            Egresos egreso = db.Egresos.Find(idEgreso);
            foreach (var det in egreso.DetalleEgresos)
            {
                total += det.cantidad * det.monto;
            }
            if (egreso.tipoFactura == tiposFacturas.A)//iva discriminado
            {
                var resultadoiva = total * (egreso.iva / 100);
                decimal conceptos;
                if (egreso.conceptosNoGravados != null)
                {
                    conceptos = Convert.ToDecimal (egreso.conceptosNoGravados);
                }
                else
                    { conceptos = 0; }

                
                var ingresosbrutos = (total + conceptos )  * (egreso.ingresosBrutos / 100);
                var impuestosinternos = total * ((egreso.impuestosInternos / 100));
                var percepcioniva = total * ((egreso.percepcionIva / 100));

                total = total + resultadoiva + ingresosbrutos + impuestosinternos + percepcioniva + conceptos;
            }
            return (total.ToString());
        }

        //devuelve el total bruto de un egreso
        public String getTotalBrutoEgreso(int idEgreso)
        {
            decimal total = 0;
            Egresos egreso = db.Egresos.Find(idEgreso);
            foreach (var det in egreso.DetalleEgresos)
            {
                total += det.cantidad * det.monto;
            }           
                if(egreso.tipoFactura==tiposFacturas.B)
                {
               var iva = ((total / ((egreso.iva / 100) + 1)) - total) * -1;

                total = total - iva;
            }
            
            return (total.ToString());
        }

        //devuelve el saldo de un egreso
        public String getSaldoEgreso(int idEgreso)
        {
            decimal total =  Convert.ToDecimal(getTotalEgreso(idEgreso));
            decimal pagado = Convert.ToDecimal(getPagadoEgreso(idEgreso));
            decimal retenido = Convert.ToDecimal(getRetenidoEgreso(idEgreso));
            decimal saldo = total - (pagado + retenido);

            return saldo.ToString();
        }
        

        //devuelve el total pagado de un egreso
        public String getPagadoEgreso(int idEgreso)
        {
            Egresos egreso = db.Egresos.Find(idEgreso);
            decimal totalPagado = 0;

            foreach (var det in egreso.Pagos.Where(a => a.idEgreso == egreso.idEgreso))
            {
                totalPagado += det.monto;
            }
            return totalPagado.ToString();
        }


        //devuelve el total pagado de un egreso
        public String getRetenidoEgreso(int idEgreso)
        {
            Egresos egreso = db.Egresos.Find(idEgreso);
            decimal totalRetenido = 0;

            foreach (var det in egreso.Pagos.Where(a => a.idEgreso == egreso.idEgreso))
            {
                totalRetenido +=Convert.ToDecimal( det.retenido);
            }
            return totalRetenido.ToString();
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
