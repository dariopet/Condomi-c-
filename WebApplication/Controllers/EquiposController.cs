using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication;
using WebApplication.CustomFilters;
using Zen.Barcode;

namespace WebApplication.Controllers
{
    public class EquiposController : Controller
    {
        private CondomiBaseEntities db = new CondomiBaseEntities();

        // GET: Equipos
       
        public ActionResult Index()
        {
            var equipos = db.Equipos.Include(e => e.ModelosEquipos);
            return View(equipos.ToList());
        }

        // GET: Equipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(equipos);
        }

        //genero la imagen con la libreria zen
        public ActionResult codigoBarras(String texto)
        {
            Image img = new Bitmap(1, 1);
            Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
            img=barcode.Draw(texto, 75,1);
            
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return File(ms.ToArray(), "image/png");
        }

       ////dibujo el codigo de barras con la fuenta de codigos
       // public ActionResult DrawText(String texto)
       // {
       //     //first, create a dummy bitmap just to get a graphics object
       //     Image img = new Bitmap(1, 1);
       //     Graphics drawing = Graphics.FromImage(img);

       //     System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
       //     string path = Server.MapPath("~/AdminLTE/fonts/code128.ttf");
       //     privateFonts.AddFontFile(path);
       //     System.Drawing.Font font = new Font(privateFonts.Families[0], 20, FontStyle.Regular);           

       //     //measure the string to see how big the image needs to be
       //     SizeF textSize = drawing.MeasureString(texto, font);

       //     //free up the dummy image and old graphics object
       //     img.Dispose();
       //     drawing.Dispose();

       //     //create a new image of the right size
       //     img = new Bitmap((int)textSize.Width, (int)textSize.Height);

       //     drawing = Graphics.FromImage(img);

       //     //paint the background
       //     drawing.Clear(Color.White);

       //     //create a brush for the text
       //     Brush textBrush = new SolidBrush(Color.Black);

       //     drawing.DrawString(texto, font, textBrush, 0, 0);

       //     drawing.Save();

       //     textBrush.Dispose();
       //     drawing.Dispose();

       //     MemoryStream ms = new MemoryStream();
       //     img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
       //     return File(ms.ToArray(), "image/png");

       // }

        // GET: Equipos/Create
        public ActionResult Create()
        {
            ViewBag.idModelo = new SelectList(db.ModelosEquipos, "idModelo", "descripcion");
            return PartialView("Create");
        }

        // POST: Equipos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEquipo,idModelo,serialNumber,codigoBarras,estado,ubicacion,numero,activo")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                equipos.activo = true;
                db.Equipos.Add(equipos);
                db.SaveChanges();
                return Json(new { success = true });
            }

            //ViewBag.idModelo = new SelectList(db.ModelosEquipos, "idModelo", "descripcion", equipos.idModelo);
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", equipos.idModelo);
            return PartialView("Create",equipos);
        }

        // GET: Equipos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", equipos.idModelo);
            return PartialView(equipos);
        }

        // POST: Equipos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEquipo,idModelo,serialNumber,codigoBarras,estado,ubicacion,numero,activo")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                equipos.editadoPor = User.Identity.GetUserId();
                equipos.fechaEditado = DateTime.Now;
                db.Entry(equipos).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.idModelo = new SelectList(db.ModelosEquipos.Where(p => p.activo == true), "idModelo", "descripcion", equipos.idModelo);
            return PartialView("Edit",equipos);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(equipos);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipos equipos = db.Equipos.Find(id);
            equipos.activo = false;
            //db.Equipos.Remove(equipos);
            db.SaveChanges();
            return Json(new { success = true });
            
        }

        // GET: Equipos/Activate/5
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(equipos);
        }

        // POST: Equipos/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(int id)
        {
            Equipos equipos = db.Equipos.Find(id);
            equipos.activo = true;
            //db.Equipos.Remove(equipos);
            db.SaveChanges();
            return Json(new { success = true });

        }



        // GET: Equipos/ActivateDelete/5
        public ActionResult ActivateDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return PartialView(equipos);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("ActivateDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateDeleteConfirmed(int id)
        {
            Equipos equipos = db.Equipos.Find(id);
            equipos.activo = false;
            //db.Equipos.Remove(equipos);
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
