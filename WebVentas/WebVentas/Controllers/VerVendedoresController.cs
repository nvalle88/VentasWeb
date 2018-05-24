using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class VerVendedoresController : Controller
    {
        // GET: VerVendedores
        public async Task<ActionResult> index()
        {
            return View();

        }
      
        public async Task<JsonResult> Cargar()
        {
            SupervisorRequest supervisorRequest = new SupervisorRequest();

            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
                int idEmpresaInt = Convert.ToInt32(idEmpresa);
                ApplicationDbContext db = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var idUsuarioActual = User.Identity.GetUserId();
                var lista = new Response();
                supervisorRequest.IdUsuario = idUsuarioActual;              
                supervisorRequest.IdEmpresa = idEmpresaInt;
                if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
                {
                   lista = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                    new Uri(WebApp.BaseAddress),
                                                                    "api/LogRutaVendedors/VendedoresPorSupervisor");                 
                }
                else if (userManager.IsInRole(idUsuarioActual, "GerenteGeneral"))
                {
                    Empresa empresa = new Empresa() {IdEmpresa= idEmpresaInt,};
                    lista = await ApiServicio.InsertarAsync(empresa, new Uri(WebApp.BaseAddress),
                                                                       "api/LogRutaVendedors/VendedoresPorEmpresa");
                }
                if (lista.IsSuccess)
                {
                    var listaVendedor = JsonConvert.DeserializeObject<List<VendedorPositionRequest>>(lista.Resultado.ToString());
                     List <VendedorPositionRequest> listaVendedores = new  List < VendedorPositionRequest >();

                    foreach (var vendedor in listaVendedor)
                    {
                        vendedor.urlFoto = string.IsNullOrEmpty(vendedor.urlFoto) != true ? vendedor.urlFoto.Replace("~", WebApp.BaseAddress) : "";
                        listaVendedores.Add(vendedor);
                    }

                    // var f = (DateTime)(listaVendedor.FirstOrDefault().fecha.Date);
                    return Json(listaVendedor);
                }
                else return Json(false);                               
            }
            catch (Exception ex)
            {
                //InicializarMensaje(Mensaje.Excepcion);
                //lista.FirstOrDefault().NumeroMenu = 1;
                return Json(false);

            }
        }
    }
}