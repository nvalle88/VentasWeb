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
    public class VendedoresController : Controller
    {

        
        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)
            {
                mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }

        

        // GET: Vendedores
        public async Task<ActionResult> VendedorIndex(string mensaje)
        {
            List<VendedorRequest> lista = new List<VendedorRequest>();
            InicializarMensaje("");

            try
            {
                lista = await ApiServicio.Listar<VendedorRequest>(new Uri(WebApp.BaseAddress)
                                                                  , "api/Vendedores/ListarVendedores");



                return View(lista);
            }
            catch
            {
                return View(lista);
            }
        }


        public async Task<ActionResult> PerfilVendedor(string mensaje)
        {
            List<VendedorRequest> lista = new List<VendedorRequest>();
            InicializarMensaje("PerfilVendedor");

            try
            {
                lista = await ApiServicio.Listar<VendedorRequest>(new Uri(WebApp.BaseAddress)
                                                                  , "api/Vendedores/ListarVendedores");



                return View(lista);
            }
            catch
            {
                return View(lista);
            }
        }


    }
}