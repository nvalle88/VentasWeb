using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    public class LandingsController : Controller
    {
        // GET: Landings
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EnviarCorreo(InfoRequest info)
        {
            var respuesta = await ApiServicio.InsertarAsync<Response>(info, new Uri(WebApp.BaseAddress)
                                                                      , "api/Informacion/Generar");

            return 
            RedirectToAction("Index");
        }
    }
}