using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication;

namespace Condomi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Inicio()
        {
            return View();
        }
      
        
        private CondomiBaseEntities db = new CondomiBaseEntities();

        private List<Events> GetEvents()
        {
            var eventos = db.Eventos.Include(e => e.AspNetUsers).Include(e => e.Terceros);
            List<Events> eventList = new List<Events>();
            List<Eventos> eventosL = new List<Eventos>();
            eventosL = eventos.Where(a=>a.idEvento!=0 && a.activo==true).ToList();

            foreach (var evento in eventosL)
            {
                if (evento.activo == true)
                {
                    Events evCalendario = new Events();
                    evCalendario.id = evento.idEvento.ToString();
                    evCalendario.title = evento.descripcion +" en "+ evento.jurisdiccion;
                    evCalendario.start = evento.fechaInicio.ToString("s");
                    evCalendario.end = evento.fechaFin.ToString("s");
                    switch (evento.estado){
                        case estadoEvento.Aceptado: evCalendario.color = "#2ECC40";
                            break;
                        case estadoEvento.Cumplido:evCalendario.color = "#FF851B";
                            break;
                        case estadoEvento.Finalizado:evCalendario.color="#FF4136";
                            break;
                        case estadoEvento.Presupuestado:evCalendario.color = "#0074D9";
                            break;
                    }
                    evCalendario.status = evento.estado.ToString ();
                    evCalendario.allDay = true;
                    eventList.Add(evCalendario);
                }
            }
            return eventList;        }

        public ActionResult GetEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            //Get the events
            //You may get from the repository also
            var eventList = GetEvents();

            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
              

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
