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
    public class SupervisorController : Controller
    {

        // GET: Supervisor
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)
            {
                mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }

        public async Task<ActionResult> Index(string mensaje)
        {
            List<SupervisorRequest> lista = new List<SupervisorRequest>();
            InicializarMensaje("");

            try
            {
                lista = await ApiServicio.Listar<SupervisorRequest>(new Uri(WebApp.BaseAddress)
                                                                  , "api/Supervisor/ListarSupervisores");
                return View(lista);
            }
            catch
            {
                return View(lista);
            }
        }
        public async Task<ActionResult> Create(string mensaje)

        {

            ViewData["IdPais"] = new SelectList(await ApiServicio.Listar<UsuarioRequest>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");

            

            InicializarMensaje(mensaje);

            return View();

        }
    }
}