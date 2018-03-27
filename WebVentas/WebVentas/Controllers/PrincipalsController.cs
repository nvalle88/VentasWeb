using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebVentas.Controllers
{
    public class PrincipalsController : Controller
    {

        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)
            {
                mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }


        public ActionResult Index(string mensaje)
        {

            InicializarMensaje(mensaje);
            return View();
        }
    }
}