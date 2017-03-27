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
using WebApplication.CustomFilters;

namespace WebApplication.Controllers
{
    public class SaldoIvaController : Controller
    {
        private List<SaldoIva> listaIva = new List<SaldoIva>();

        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Terceros
        [AuthLog(Roles = "Administrador, Contabilidad total, Contabilidad parcial")]
        public  ActionResult Index()
        {
            return View();
        }

        public ActionResult EntreFechas(String inicio, String fin)
        {
            DateTime iniciof =Convert.ToDateTime(inicio);
            DateTime finf = Convert.ToDateTime(fin);
            var egreso = db.Egresos.Where(a => a.fechaGenerada >= iniciof && a.fechaGenerada <= finf && a.enBlanco == true && a.activo==true);
            var ingreso = db.Ingresos.Where(a => a.fechaGenerada >= iniciof && a.fechaGenerada <= finf && a.enBlanco == true && a.activo == true);

            foreach (Egresos egr in egreso)
            {
                getIvaCredito(egr);
            }
            foreach (Ingresos ing in ingreso)
            {
                getIvaDebito(ing);
            }
            return PartialView(listaIva.OrderByDescending(a=>a.fecha) );
        }

        public void getIvaCredito(Egresos eg)
        {            
            decimal monto = 0;          
            decimal iva = 0;         
           
                var detalle = db.DetalleEgresos.Where(a => a.idEgreso == eg.idEgreso);
                if (eg.tipoFactura == tiposFacturas.A)
                {
                    foreach (DetalleEgresos de in detalle)
                    {
                        monto += de.monto;
                    }
                    iva = monto * (eg.iva / 100);
                   
                }
                if (eg.tipoFactura == tiposFacturas.B)
                {
                    foreach (DetalleEgresos de in detalle)
                    {
                        monto += de.monto;
                    }
                    iva = ((monto / ((eg.iva / 100) + 1))- monto)*- 1;
                   
                }
            SaldoIva saldo = new SaldoIva();
            saldo.egreso = eg;
            saldo.fecha = eg.fechaGenerada;
            saldo.ivaCredito = iva;
            listaIva.Add(saldo);                        

        }


        public void getIvaDebito(Ingresos ing)
        {
           
            decimal monto = 0;         
            decimal iva = 0;
          

                var detalle = db.DetalleIngresos.Where(a => a.idIngreso == ing.idIngreso);
                if (ing.tipoFactura == tiposFacturas.A)
                {
                    foreach (DetalleIngresos de in detalle)
                    {
                        monto += de.monto;
                    }
                    iva = monto * (ing.iva / 100);
                   
                }
                if (ing.tipoFactura == tiposFacturas.B)
                {
                    foreach (DetalleIngresos de in detalle)
                    {
                        monto += de.monto;
                    }
                    iva = ((monto / ((ing.iva / 100) + 1)) - monto) * -1;
                    
                }
              SaldoIva saldo = new SaldoIva();
               saldo.ingreso = ing;
               saldo.ivaDebito = iva;
            saldo.fecha = ing.fechaGenerada;
            listaIva.Add(saldo);   
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
