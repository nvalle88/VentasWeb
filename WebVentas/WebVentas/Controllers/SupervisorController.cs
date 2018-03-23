using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
               InicializarMensaje("");

                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
                var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };


                var lista = await ApiServicio.Listar<SupervisorRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                                  , "api/Supervisor/ListarSupervisores");
                return View(lista);
           
            
        }
        public ActionResult Create(string mensaje)

        {
            InicializarMensaje(mensaje);
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Create(SupervisorRequest supervisorRequest)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(supervisorRequest);
            }
            Response response = new Response();
            try
            {

            ApplicationDbContext db = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };
            var user = new ApplicationUser { UserName = supervisorRequest.Correo, Email = supervisorRequest.Correo,
            Identificacion = supervisorRequest.Identificacion,Apellidos =supervisorRequest.Apellidos,Nombres = supervisorRequest.Nombres, Estado =0,
            Direccion=supervisorRequest.Direccion,Telefono = supervisorRequest.Telefono, IdEmpresa = empresaActual.IdEmpresa };
            var result = await userManager.CreateAsync(user, "A123345.1a");
            db.SaveChanges();
                if (result != null) {

                    supervisorRequest.IdEmpresa = Convert.ToInt32(idEmpresa);
                    supervisorRequest.IdUsuario = user.Id;
                    response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Supervisor/InsertarSupervisor");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
            ViewData["Error"] = response.Message;

            return View(supervisorRequest);

        }
    }
}