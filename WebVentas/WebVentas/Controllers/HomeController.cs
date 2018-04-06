using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Supervisor"))
            {

                return RedirectToAction("Estadisticas","Supervisor");
            }
            if (User.IsInRole("GerenteGeneral"))
            {
               
               return RedirectToAction("MapaCalor", "InformacionGerencial");
            }

            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}