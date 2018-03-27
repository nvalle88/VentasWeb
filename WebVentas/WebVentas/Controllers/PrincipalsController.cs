using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebVentas.Models;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

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


        public async Task<ActionResult> Index(string mensaje)
        {

            InicializarMensaje(mensaje);
            
            return View();
            
        }


        public async Task<JsonResult> ListaVendedores()
        {

            var lista = new List<UbicacionPersonaRequest>();

            SupervisorRequest supervisorRequest = new SupervisorRequest();
            VendedorRequest vendedorRequest = new VendedorRequest();

            vendedorRequest.IdSupervisor = 8; //va 0 para probar hasta que este el rol supervisor!!!!!!

            int idEmpresaInt = 0;

            try
            {

                ApplicationDbContext db = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                // obtener el idEmpresa
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                // convertir el idEmpresa a int
                idEmpresaInt = Convert.ToInt32(idEmpresa);


                //** agregar el idEmpresa al vendedorRequest **
                vendedorRequest.idEmpresa = idEmpresaInt;


                var idUsuarioActual = User.Identity.GetUserId();

                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;
                

                if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
                {

                    // obtener el Id del supervisor
                    Response response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                    supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());


                    //** agregar el id del supervisor al vendedorRequest **
                    vendedorRequest.IdSupervisor = supervisorRequest.IdSupervisor;
                }

                   


                
                

                lista = await ApiServicio.ObtenerElementoAsync1<List<UbicacionPersonaRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarVendedoresConUbicacionPorSupervisor");
                

                return Json(lista);

            }
            catch (Exception ex)
            {
                
                return Json(false);
                
            }
            
        }
        /*
        public async Task<JsonResult> ListaClientes()
        {
            var lista = new List<ClienteRequest>();

            VendedorRequest vendedorRequest = new VendedorRequest();
            int idEmpresaInt = 0;

            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

                vendedorRequest.idEmpresa = idEmpresaInt;


                lista = await ApiServicio.ObtenerElementoAsync1<List<UbicacionPersonaRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarVendedoresConUbicacion");



                return Json(lista);

            }
            catch (Exception ex)
            {

                return Json(false);

            }

        }
        */
    }
}