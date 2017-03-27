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

namespace WebApplication.Controllers
{
    public class EquiposNoRegresaronController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: EquiposNoRegresaron/index/?5        
        public ActionResult Index()
        {
            var movimientosEquipos = db.MovimientosEquipos.Include(m => m.AspNetUsers).Include(m => m.Equipos).Include (m=>m.Eventos);              
           
                return View( movimientosEquipos.Where(p=>p.tipoMovimiento ==tipoMovimiento.aEvento && p.Eventos.fechaFin<DateTime.Now && p.Equipos.estado==Estados.Trabajando && p.idEvento!=null && p.Eventos.estado!=estadoEvento.Presupuestado ).OrderByDescending(a => a.fecha).ToList());
            
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
