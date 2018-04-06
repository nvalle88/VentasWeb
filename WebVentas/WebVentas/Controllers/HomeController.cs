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
using VentaServicios.Utils;
using WebVentas.Models;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    [Authorize(Roles = "GerenteGeneral,Supervisor")]
    public class HomeController : Controller
    {

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }

        #region Estadisticos
        public async Task<ActionResult> Estadisticas()
        {
            try
            {
                InicializarMensaje("");
                SupervisorRequest supervisorRequest = new SupervisorRequest();
                int idEmpresaInt = 0;
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;


                idEmpresaInt = Convert.ToInt32(idEmpresa);
                supervisorRequest.IdEmpresa = idEmpresaInt;

                ApplicationDbContext db = new ApplicationDbContext();
                Response response = new Response();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var idUsuarioActual = User.Identity.GetUserId();
                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;
                //metodo buscar supervisor

                response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                supervisorRequest.IdSupervisor = supervisorRequest.IdSupervisor;


                var estadisticoVendedorRequest = await ApiServicio.ObtenerElementoAsync1<EstadisticoSupervisorRequest>(supervisorRequest,
                    new Uri(WebApp.BaseAddress),
                    "api/Compromiso/VerEstadisticosPorSupervisor");
                supervisorRequest.estadisticoSupervisorRequest = estadisticoVendedorRequest;
                return View(supervisorRequest);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View();
            }
        }
        #endregion
        public ActionResult Index()

        {
            if (User.IsInRole("Supervisor"))
            {

                return RedirectToAction("Estadisticas", "Home");
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