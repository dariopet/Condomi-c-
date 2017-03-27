using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
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
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AspNetUsersController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: AspNetUsers
        [AuthLog(Roles = "Administrador")]
        public ActionResult Index()
        {
           
            return View(db.AspNetUsers.ToList());
        }

            ApplicationDbContext context;

        [AuthLog(Roles = "Administrador")]
        public ActionResult AsignarRol(string id)
        {

            context = new ApplicationDbContext();
            RolUsuarios rol = new RolUsuarios();
            rol.idUsuario = id;
            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            return PartialView(rol);
        }

        // POST: Egresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarRol(RolUsuarios rol)
        {
            if (ModelState.IsValid)
            {
             
                var _context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                //borro todos los roles
                string[] allUserRoles = UserManager.GetRoles(rol.idUsuario).ToArray();
                UserManager.RemoveFromRoles(rol.idUsuario, allUserRoles);

                                //le agrego el nuevo
                UserManager.AddToRole(rol.idUsuario, rol.Name);
               
                return Json(new { success = true });
            }

            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            return PartialView(rol);
        }



        public  String GetUsersInRole( string userid)
        {

            //recibo un id de un usuario, busco cual es el rol id y con ese id busco el nombre del rol
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var role = userManager.FindById(userid).Roles.FirstOrDefault().RoleId;
            var user = roleManager.FindById(role).Name.ToString();
            return user;
            
           
        }

        // GET: Egresos/ResetearPassword/5
        [AuthLog(Roles = "Administrador")]
        public ActionResult ResetearPassword(string id)
        {
            RolUsuarios rol = new RolUsuarios();
            rol.idUsuario = id;
        
            return PartialView(rol);
        }

        // POST: Egresos/ResetearPassword/5
        [HttpPost, ActionName("ResetearPassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetearPasswordConfirmed(RolUsuarios rol)
        {
            var _context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));         
            UserManager.RemovePassword(rol.idUsuario);
            UserManager.AddPassword(rol.idUsuario, "123456");
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
