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
    public class ClientesController : Controller
    {

        
        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)
            {
                mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }

        /*
        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }
        */

        // GET: Clientes
        public async Task<ActionResult> Index(string mensaje)
        {
            List<Cliente> lista = new List<Cliente>();
            InicializarMensaje("");

            try
            {
                lista = await ApiServicio.Listar<Cliente>(new Uri(WebApp.BaseAddress)
                                                                  , "api/Clientes/ListarClientes");



                return View(lista);
            }
            catch
            {
                return View(lista);
            }
        }

    }
}