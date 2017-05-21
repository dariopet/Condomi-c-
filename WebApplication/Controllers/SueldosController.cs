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
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class SueldosController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Sueldos
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult EntreFechas(String inicioT, String finT)
        {
            DateTime inicio = Convert.ToDateTime(inicioT);
            DateTime fin = Convert.ToDateTime(finT);
            var sueldos = db.Sueldos.Include(s => s.AspNetUsers).Include(s => s.Cuentas).Include(s => s.Terceros);

            return PartialView(sueldos.Where(a => a.fecha >= inicio && a.fecha <= fin).OrderByDescending(a => a.fecha));
        }

        // GET: Sueldos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sueldos sueldos =  db.Sueldos.Find(id);
            if (sueldos == null)
            {
                return HttpNotFound();
            }
            return PartialView(sueldos);
        }

        // GET: Sueldos/Create
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p=>p.idTercero==0) , "idCuenta", "descripcion");                                             
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre").OrderBy(p => p.Text);
            Sueldos sueldo = new Sueldos();
            sueldo.idCuenta = 1;
                                   
            return PartialView(sueldo);
        }

        // POST: Sueldos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idSueldo,base,monto,idTercero,periodo,fecha,idUsuario,idCuenta")] Sueldos sueldos)
        {
            if (ModelState.IsValid)
            {
                Cuentas cuen = db.Cuentas.Find(sueldos.idCuenta);
                cuen.fondos -= sueldos.monto;
                sueldos.fecha = DateTime.Now;
                sueldos.idUsuario= User.Identity.GetUserId();
                db.Sueldos.Add(sueldos);
                 db.SaveChanges();
                 return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", sueldos.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0), "idCuenta", "descripcion", sueldos.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", sueldos.idTercero).OrderBy(p => p.Text);
            return PartialView(sueldos);
        }

        // GET: Sueldos/Edit/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sueldos sueldos =  db.Sueldos.Find(id);
            if (sueldos == null)
            {
                return HttpNotFound();
            }
            //guardo los datos originales temporalmente para tener acceso despues
            TempData["cuenta"] = sueldos.idCuenta;
            TempData["monto"] = sueldos.monto;
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", sueldos.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0), "idCuenta", "descripcion", sueldos.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", sueldos.idTercero).OrderBy(p => p.Text);
            return PartialView(sueldos);
        }

        // POST: Sueldos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSueldo,base,monto,idTercero,periodo,fecha,idUsuario,idCuenta")] Sueldos sueldos)
        {
            if (ModelState.IsValid)
            {
                //descuento el monto original de la cuenta original
                //busco la cuenta nueva y lo sumo
                Cuentas cuenta1 = db.Cuentas.Find(TempData["cuenta"]);
                cuenta1.fondos += Convert.ToDecimal(TempData["monto"]);
                //si cambia de cuenta la busco, sino le sumo el nuevo importe
                if (sueldos.idCuenta != ViewBag.cuenta)
                {
                    Cuentas cuenta2 = db.Cuentas.Find(sueldos.idCuenta);
                    cuenta2.fondos -= sueldos.monto;
                }
                else
                {
                    cuenta1.fondos -= sueldos.monto;
                }

                db.Entry(sueldos).State = EntityState.Modified;
                sueldos.editadoPor = User.Identity.GetUserId();
                sueldos.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", sueldos.idUsuario);
            ViewBag.idCuenta = new SelectList(db.Cuentas.Where(p => p.idTercero == 0), "idCuenta", "descripcion", sueldos.idCuenta);
            ViewBag.idTercero = new SelectList(db.Terceros.Where(p => p.activo == true && (p.idTipoTercero == 1 || p.idTipoTercero == 2)), "idTercero", "nombre", sueldos.idTercero).OrderBy(p => p.Text);
            return PartialView(sueldos);
        }

        // GET: Sueldos/Delete/5
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sueldos sueldos =  db.Sueldos.Find(id);
            if (sueldos == null)
            {
                return HttpNotFound();
            }
            return PartialView(sueldos);
        }

        // POST: Sueldos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sueldos sueldos =  db.Sueldos.Find(id);
            
             db.SaveChanges();
            return Json(new { success = true });
        }
      
        public JsonResult getDatosSueldo(int idTercero, string periodo)
        {
            int horas;
            decimal adelantos;
            decimal ultimoSueldo;
            decimal cobradoPeriodoAnterior;

            //horas no trabajadas
            var data = db.HorasNoTrabajadas.Where(a => a.idTercero == idTercero && ((a.periodo.Month.ToString() + "/" + a.periodo.Year.ToString()).Equals(periodo) || ("0"+ a.periodo.Month.ToString() + "/" + a.periodo.Year.ToString()).Equals(periodo)));
            if (data.Count() != 0)
            {
                horas = data.Sum(d => d.cantidad);                
            }
            else
            {
                horas = 0;
            }
            //adelantos recibidos
            var data2 = db.Sueldos.Where(a => a.idTercero == idTercero && ((a.periodo.Month.ToString() + "/" + a.periodo.Year.ToString()).Equals(periodo) || ("0" + a.periodo.Month.ToString() + "/" + a.periodo.Year.ToString()).Equals(periodo)));
            if (data2.Count() != 0)
            {
                adelantos = data2.Sum(d => d.monto);            
            }
            else
            {
                adelantos = 0;                
            }
            //ultimo sueldo
            var data3 = db.Sueldos.OrderByDescending(a=>a.fecha).Where(a => a.idTercero == idTercero);
            if (data3.Count() != 0)
            {               
                ultimoSueldo = data3.FirstOrDefault().@base;
            }
            else
            {              
                ultimoSueldo = 0;
            }
            //cobrado periodo anterior
            DateTime periodoTemp;

            periodoTemp = new DateTime(Convert.ToInt32(periodo.Substring(periodo.IndexOf("/")+1, 4)), Convert.ToInt32(periodo.Substring(0, periodo.IndexOf("/"))), 15);
            periodoTemp=periodoTemp.AddMonths(-1);
            String periodoAnterior = periodoTemp.Month.ToString() + "/" + periodoTemp.Year.ToString();

            var data4 = db.Sueldos.Where(a => a.idTercero == idTercero &&( (a.periodo.Month + "/" + a.periodo.Year).Equals(periodoAnterior) || ("0"+ a.periodo.Month + "/" + a.periodo.Year).Equals(periodoAnterior)));
            if (data4.Count() != 0)
            {
                cobradoPeriodoAnterior  = data4.Sum(d => d.monto);
            }
            else
            {
                cobradoPeriodoAnterior = 0;
            }
            return Json(new { horasNoTrabajadas = horas, totalAdelantos = adelantos , ultimoPago = ultimoSueldo, cobradoAnterior= cobradoPeriodoAnterior }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPrecioHora(decimal sueldo, string periodo)
        {
            decimal precio=0;
            int diasHabiles=0;

            DateTime inicio=Convert.ToDateTime(periodo);
            DateTime fin = Convert.ToDateTime(periodo);

            DateTime start = new DateTime(inicio.Year, inicio.Month , 1);
            DateTime stop = new DateTime(fin.Year, fin.Month, DateTime.DaysInMonth(fin.Year,fin.Month));


            while (start <= stop)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasHabiles += 1;
                }
                start = start.AddDays(1);
            }
            precio = (sueldo / diasHabiles) / 8;

            precio=Convert.ToDecimal( precio.ToString("0.##"));

            return Json(new { precioHora=precio }, JsonRequestBehavior.AllowGet);
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
