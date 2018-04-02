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
    public class CompromisosController : Controller
    {
        // GET: Compromisos
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
        #region Compromiso
        public async Task<ActionResult> Index()
        {
            //bUSCA LA EMPRESA
            var idEmpresaInt = 0;
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);

                return View();
            }
            //FIN
            SupervisorRequest supervisorRequest = new SupervisorRequest();

            ApplicationDbContext db = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var idUsuarioActual = User.Identity.GetUserId();

            supervisorRequest.IdUsuario = idUsuarioActual;
            supervisorRequest.IdEmpresa = idEmpresaInt;

            if (userManager.IsInRole(idUsuarioActual, "GerenteGeneral"))
            {
                Response response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/obtenerSupervisorPorIdUsuario");

                supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                var userWithClaims = (ClaimsPrincipal)User;
                try
                {
                    var lista = await ApiServicio.ObtenerElementoAsync1<SupervisorRequest>(supervisorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarVendedoresGerente");
                    //var objeto = JsonConvert.DeserializeObject<SupervisorRequest>(lista.ListaVendedores.ToString());
                    ViewBag.IdVendedor = new SelectList(lista.ListaVendedores, "IdVendedor", "NombreApellido");
                    var vista1 = new SupervisorRequest { FechaInicio = DateTime.Now, FechaFin = DateTime.Now, Listarcompromiso = new List<CompromisoRequest>() };

                    return View(vista1);

                }
                catch (Exception ex)
                {

                    ex.ToString();

                    return View();
                }

                ;
            }
            var vista = new SupervisorRequest { FechaInicio = DateTime.Now, FechaFin = DateTime.Now, Listarcompromiso = new List<CompromisoRequest>() };
            return View(vista);
        }

        [HttpPost]
        public async Task<ActionResult> Index(SupervisorRequest supervisorRequest)
        {

            //bUSCA LA EMPRESA
            var idEmpresaInt = 0;
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View();
            }
            //FIN
            SupervisorRequest supervisorRequest1 = new SupervisorRequest();

            ApplicationDbContext db = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var idUsuarioActual = User.Identity.GetUserId();

            supervisorRequest1.IdUsuario = idUsuarioActual;
            supervisorRequest1.IdEmpresa = idEmpresaInt;

            if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
            {
                Response response = await ApiServicio.InsertarAsync(supervisorRequest1,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/obtenerSupervisorPorIdUsuario");

                supervisorRequest1 = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                supervisorRequest.IdSupervisor = supervisorRequest1.IdSupervisor;
                var lista = await ApiServicio.ObtenerElementoAsync1<SupervisorRequest>(supervisorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vista/ListarVisitas");
                supervisorRequest.Listarcompromiso = lista.Listarcompromiso;

                var listavendedor = await ApiServicio.ObtenerElementoAsync1<SupervisorRequest>(supervisorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarVendedoresGerente");
                ViewBag.IdVendedor = new SelectList(listavendedor.ListaVendedores, "IdVendedor", "NombreApellido");

                supervisorRequest.ListaVendedores = listavendedor.ListaVendedores;
                return View(supervisorRequest);
            }
            return View();

        }

        #endregion
    }
}