using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VentaServicios.Utils;
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
            VendedorRequest vr = new VendedorRequest();
            Response response = new Response();

            InicializarMensaje("");


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                vr.idEmpresa = Convert.ToInt32( idEmpresa );
            }
            catch (Exception ex) {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
            }


            try
            {
                

                lista = await ApiServicio.ObtenerElementoAsync1<List<VendedorRequest>>(vr,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarVendedores");

                
                return View(lista);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(lista);
            }
        }


        public async Task<ActionResult> PerfilVendedor(string mensaje,int idVendedor)
        {
            VendedorRequest vendedor = new VendedorRequest();
            vendedor.IdVendedor = idVendedor;

            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                vendedor.idEmpresa = Convert.ToInt32(idEmpresa);
            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
            }

             

            InicializarMensaje("PerfilVendedor");

            try
            {

                vendedor = await ApiServicio.ObtenerElementoAsync1<VendedorRequest>(vendedor,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarClientesPorVendedor");


                InicializarMensaje("all ok");
                return View(vendedor);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(vendedor);
            }
        }


    }
}