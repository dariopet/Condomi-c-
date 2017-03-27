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
    public class MovimientosEquiposController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: MovimientosEquipos/index/?5    
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Index(int? id)
        {
            var movimientosEquipos = db.MovimientosEquipos.Include(m => m.AspNetUsers).Include(m => m.Equipos);
            if (id != null)
            {
                ViewBag.unEquipo = "si";                
                ViewBag.descripcion = db.Equipos.Find(id).ModelosEquipos.descripcion;
                return View( movimientosEquipos.Where(p=>p.idEquipo == id).OrderByDescending(p=>p.fecha ).ToList());
            }
            else
            {
                ViewBag.unEquipo = "no";
                return View( movimientosEquipos.OrderByDescending(p=>p.fecha ).ToList());
            }
        }

        // GET: MovimientosEquipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientosEquipos movimientosEquipos =  db.MovimientosEquipos.Find(id);
            if (movimientosEquipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(movimientosEquipos);
        }

        // GET: MovimientosEquipos/Create
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier");
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.fechaFin >=DateTime.Now && p.estado ==estadoEvento.Aceptado ).OrderByDescending(p => p.fechaInicio), "idEvento", "descripcion");
            //ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true ), "idEquipo", "codigoBarras");
            return PartialView();
        }

        // POST: MovimientosEquipos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idMovimientoEquipo,descripcion,fecha,idUsuario,idEvento,idEquipo,tipoMovimiento, codigoBarras, cantidad")] MovimientosEquipos movimientosEquipos)
        {
            int cantidad = Convert.ToInt32 ( Request["cantidadAMano"]);
            Equipos eq;
            int x;
            int contadorExitosos = 0;

            if (ModelState.IsValid)
            {
                if (movimientosEquipos.tipoMovimiento == tipoMovimiento.aEvento)//va a evento
                {
                    for (x = 0; x < movimientosEquipos.cantidad ; x++)
                    { 
                      var equipo = from a in db.Equipos
                      where a.estado == Estados.Deposito && a.activo == true && String.Equals(a.codigoBarras, movimientosEquipos.codigoBarras)
                      select a;
                    if (!equipo.Any())
                    {
                            return Content("<div class='alert alert-danger' role='alert'> Sólo hay " + contadorExitosos.ToString() + " equipos de ese tipo en el depósito </div>");
                        }
                        contadorExitosos += 1;
                        eq = equipo.First();
                        eq.estado = Estados.Trabajando;
                        movimientosEquipos.idUsuario = User.Identity.GetUserId();
                        movimientosEquipos.idEquipo = eq.idEquipo;
                        db.MovimientosEquipos.Add(movimientosEquipos);
                         db.SaveChanges();
                    }
                   
                }
            else if (movimientosEquipos.tipoMovimiento == tipoMovimiento.aReparacion)//va a reparacion
            {
                    for (x = 0; x < movimientosEquipos.cantidad; x++)
                    {
                        var equipo = from a in db.Equipos
                                     where a.estado == Estados.Deposito && a.activo == true && a.codigoBarras == movimientosEquipos.codigoBarras
                                     select a;
                        if (!equipo.Any())
                        {
                            return Content("<div class='alert alert-danger' role='alert'> Sólo hay " + contadorExitosos.ToString() + " equipos de ese tipo en el depósito </div>");
                        }
                        eq = equipo.First();
                        contadorExitosos += 1;
                        eq.estado = Estados.Reparación;
                        movimientosEquipos.idEvento = null;
                        movimientosEquipos.idUsuario = User.Identity.GetUserId();
                        movimientosEquipos.idEquipo = eq.idEquipo;
                        db.MovimientosEquipos.Add(movimientosEquipos);
                         db.SaveChanges();
                    }
            }

            else//vuelve no se sabe de donde
            {
                var equipo = from a in db.Equipos
                             where a.estado != Estados.Deposito && a.activo == true && a.codigoBarras == movimientosEquipos.codigoBarras
                             select a;
                    for (x = 0; x < movimientosEquipos.cantidad; x++)
                    {
                        if (!equipo.Any())
                        {
                            return Content("<div class='alert alert-danger' role='alert'> Sólo hay " + contadorExitosos.ToString() + " equipos de ese tipo fuera del depósito </div>");
                        }
                        eq = equipo.First();
                        contadorExitosos += 1;
                        eq.estado = Estados.Deposito;
                        movimientosEquipos.idEvento = null;
                        movimientosEquipos.idUsuario = User.Identity.GetUserId();
                        movimientosEquipos.idEquipo = eq.idEquipo;
                        db.MovimientosEquipos.Add(movimientosEquipos);
                         db.SaveChanges();
                    }
            }
               
                return Json(new { success = true });
                
            }

            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", movimientosEquipos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.fechaFin >= DateTime.Now).OrderByDescending(p=>p.fechaInicio), "idEvento", "descripcion", movimientosEquipos.idEvento);
            //ViewBag.idEquipo = new SelectList(db.Equipos.Where(p => p.activo == true), "idEquipo", "codigoBarras", movimientosEquipos.idEquipo);
            return PartialView(movimientosEquipos);
        }



        // GET: MovimientosEquipos/Edit/5
        [AuthLog(Roles = "Administrador, Depósito")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientosEquipos movimientosEquipos =  db.MovimientosEquipos.Find(id);
            if (movimientosEquipos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", movimientosEquipos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.fechaFin >= DateTime.Now).OrderByDescending(p => p.fechaInicio), "idEvento", "descripcion", movimientosEquipos.idEvento);
            return PartialView(movimientosEquipos);
        }

        // POST: MovimientosEquipos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMovimientoEquipo,descripcion,fecha,idUsuario,idEvento,idEquipo,tipoMovimiento, codigoBarras")] MovimientosEquipos movimientosEquipos)
        {
            //pongo un valor valido y elimino el error del modelstate, xq es una propiedad q esta fuera de la base
            movimientosEquipos.cantidad = 1;
            ModelState["cantidad"].Errors.Clear();
            if (ModelState.IsValid)
            {
                db.Entry(movimientosEquipos).State = EntityState.Modified;
                movimientosEquipos.editadoPor = User.Identity.GetUserId();
                movimientosEquipos.fechaEditado = DateTime.Now;
                 db.SaveChanges();
                return Json(new { success = true });
                
            }
            ViewBag.idUsuario = new SelectList(db.AspNetUsers, "Id", "NameIdentifier", movimientosEquipos.idUsuario);
            ViewBag.idEvento = new SelectList(db.Eventos.Where(p => p.activo == true && p.fechaFin >= DateTime.Now).OrderByDescending(p => p.fechaInicio), "idEvento", "descripcion", movimientosEquipos.idEvento);
           
            return PartialView(movimientosEquipos);
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
