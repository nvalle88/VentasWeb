using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using SmartAdminMvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VentaServicios.Utils;
using WebVentas.Models;
using WebVentas.ObjectModel;
using WebVentas.ObjectRequest;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    [Authorize(Roles = "Supervisor,GerenteGeneral")]
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

        
        public async Task<ActionResult> VendedorIndex(string mensaje)
        {
            List<VendedorRequest> lista = new List<VendedorRequest>();
            VendedorRequest vendedorRequest = new VendedorRequest();
            SupervisorRequest supervisorRequest = new SupervisorRequest();

            int idEmpresaInt = 0;

            Response response = new Response();

            InicializarMensaje(mensaje);


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

                vendedorRequest.idEmpresa = idEmpresaInt;
            }
            catch (Exception ex) {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(lista);
            }


            try
            {


                ApplicationDbContext db = new ApplicationDbContext();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


                var idUsuarioActual = User.Identity.GetUserId();

                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;

                if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
                {
                    response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                    supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                    vendedorRequest.IdSupervisor = supervisorRequest.IdSupervisor;

                    lista = await ApiServicio.ObtenerElementoAsync1<List<VendedorRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarVendedoresPorSupervisor");

                }
                else //(userManager.IsInRole(idUsuarioActual, "GerenteComercial"))
                {
                    lista = await ApiServicio.ObtenerElementoAsync1<List<VendedorRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarVendedores");
                }


                return View(lista);
            }
            catch(Exception ex)
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(lista);
            }
        }


        public async Task<ActionResult> PerfilVendedor(string mensaje,int idVendedor)
        {
            SupervisorRequest supervisorRequest = new SupervisorRequest();
            VendedorRequest vendedor = new VendedorRequest();
            vendedor.IdVendedor = idVendedor;

            int idEmpresaInt = 0;

            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

                vendedor.idEmpresa = idEmpresaInt;
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

                var estadisticoVendedorRequest = await ApiServicio.ObtenerElementoAsync1<EstadisticoVendedorRequest>(vendedor,
                    new Uri(WebApp.BaseAddress),
                    "api/Agendas/VerEstadisticosVendedor");

                vendedor.estadisticoVendedorRequest = estadisticoVendedorRequest;


                InicializarMensaje("");
                return View(vendedor);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(vendedor);
            }
        }


        public async Task<ActionResult> CrearVendedor(string mensaje)
        {
            VendedorRequest vendedorRequest = new VendedorRequest();

            InicializarMensaje(mensaje);

            return View(vendedorRequest);
        }

        

        [HttpPost]
        public async Task<ActionResult> CrearVendedor(VendedorRequest vendedorRequest)
        {
            InicializarMensaje("");

            int idEmpresaInt = 0;

            SupervisorRequest supervisorRequest = new SupervisorRequest();

            var guardar = false;



            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
                

                idEmpresaInt = Convert.ToInt32(idEmpresa);
                vendedorRequest.idEmpresa = idEmpresaInt;
                vendedorRequest.TiempoSeguimiento = 5;
            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(vendedorRequest);
            }


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);
                return View(vendedorRequest);
            }

            Response response = new Response();

            try
            {

                ApplicationDbContext db = new ApplicationDbContext();
                
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                
                
                var idUsuarioActual = User.Identity.GetUserId();

                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;

                if ( userManager.IsInRole(idUsuarioActual  , "Supervisor") )
                {
                    response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                    supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                    vendedorRequest.IdSupervisor = supervisorRequest.IdSupervisor;

                    guardar = true;
                }

                if (userManager.IsInRole(idUsuarioActual, "GerenteComercial"))
                {
                    guardar = true;

                }
                

                if (guardar == false)
                {
                    InicializarMensaje("No tiene permisos para agregar un nuevo vendedor");
                    return View(vendedorRequest);
                }


                var user = new ApplicationUser
                {
                    UserName = vendedorRequest.Correo,
                    Email = vendedorRequest.Correo,
                    Identificacion = vendedorRequest.Identificacion,

                    Nombres = vendedorRequest.Nombres,
                    Apellidos = vendedorRequest.Apellidos,
                    Direccion = vendedorRequest.Direccion,
                    Telefono = vendedorRequest.Telefono,
                    
                    Estado = 1,
                    
                    IdEmpresa = idEmpresaInt
                };



                var result = await userManager.CreateAsync(user, "A123345.1a");
                db.SaveChanges();

                if (result != null)
                {
                    vendedorRequest.IdUsuario = user.Id;

                    
                    userManager.AddToRole(vendedorRequest.IdUsuario, "Vendedor");

                    

                    response = await ApiServicio.InsertarAsync(vendedorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/InsertarVendedor");

                    

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("VendedorIndex", new { mensaje = response.Message});
                    }
                }


                InicializarMensaje("No se ha podido crear un usuario");
                return View(vendedorRequest);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = response.Message;
                return View(vendedorRequest);
            }

            
        }



        public async Task<ActionResult> EditarVendedor(int id)
        {
            VendedorRequest vendedor = new VendedorRequest();
            vendedor.IdVendedor = id;

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



            InicializarMensaje("") ;

            try
            {

                vendedor = await ApiServicio.ObtenerElementoAsync1<VendedorRequest>(vendedor,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarClientesPorVendedor");
                
                InicializarMensaje("");
                return View(vendedor);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(vendedor);
            }
        }


        [HttpPost]
        public async Task<ActionResult> EditarVendedor(VendedorRequest vendedorRequest)
        {
            InicializarMensaje(Mensaje.GuardadoSatisfactorio);
            int idEmpresaInt = 0;


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);
                vendedorRequest.idEmpresa = idEmpresaInt;
                vendedorRequest.TiempoSeguimiento = 5;
            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(vendedorRequest);
            }


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);
                return View(vendedorRequest);
            }

            Response response = new Response();

            ApplicationDbContext db = new ApplicationDbContext();

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {

                   

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var InstanciaUsuario= await userManager.FindByIdAsync(vendedorRequest.IdUsuario);

                    InstanciaUsuario.UserName = vendedorRequest.Correo;
                    InstanciaUsuario.Email = vendedorRequest.Correo;
                    InstanciaUsuario.Identificacion = vendedorRequest.Identificacion;
                    InstanciaUsuario.Nombres = vendedorRequest.Nombres;
                    InstanciaUsuario.Apellidos = vendedorRequest.Apellidos;
                    InstanciaUsuario.Direccion = vendedorRequest.Direccion;
                    InstanciaUsuario.Telefono = vendedorRequest.Telefono;

                    InstanciaUsuario.Estado = 1;
                    InstanciaUsuario.IdEmpresa = idEmpresaInt;

                    
                    db.Entry(InstanciaUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    
                    

                    response = await ApiServicio.InsertarAsync(vendedorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/EditarVendedor");


                    if (response.IsSuccess)
                    {
                        transaction.Commit();

                        return RedirectToAction("VendedorIndex", new { mensaje = response.Message});
                    }


                    transaction.Rollback();

                    ViewData["Error"] = response.Message;
                    return View(vendedorRequest);


                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    ViewData["Error"] = response.Message;
                    return View(vendedorRequest);
                }
            }


        }

        public async Task<ActionResult> DeshabilitarVendedor(string idUsuario)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var InstanciaUsuario = await userManager.FindByIdAsync(idUsuario);
                    
                    InstanciaUsuario.Estado = 0;


                    db.Entry(InstanciaUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    transaction.Commit();
                    return RedirectToAction("VendedorIndex", new { mensaje = Mensaje.BorradoSatisfactorio });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return RedirectToAction("VendedorIndex", new { mensaje = Mensaje.Error });
                }
            }
        }


        public async Task<ActionResult> CalendarioIndex(string mensaje)
        {
            InicializarMensaje(mensaje);

            var lista = new List<VendedorRequest>();
            VendedorRequest vendedorRequest = new VendedorRequest();
            SupervisorRequest supervisorRequest = new SupervisorRequest();
            int idEmpresaInt = 0;

            var listaEventos = new List<EventoRequest>();


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

                vendedorRequest.idEmpresa = idEmpresaInt;
            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(listaEventos);
            }



            try
            {
                ApplicationDbContext db = new ApplicationDbContext();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


                var idUsuarioActual = User.Identity.GetUserId();

                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;

                if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
                {
                    Response response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                    supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                    vendedorRequest.IdSupervisor = supervisorRequest.IdSupervisor;


                }


                lista = await ApiServicio.ObtenerElementoAsync1<List<VendedorRequest>>(vendedorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarVendedoresPorSupervisor");

                ViewBag.IdVendedor = new SelectList(lista, "IdVendedor", "Nombres");


                return View(listaEventos);
            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(listaEventos);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CalendarioIndex(int idVendedor)
        {
            SupervisorRequest supervisorRequest = new SupervisorRequest();
            VendedorRequest vendedorRequest = new VendedorRequest();

            var lista = new List<VendedorRequest>();
            int idEmpresaInt = 0;


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;

                idEmpresaInt = Convert.ToInt32(idEmpresa);

                vendedorRequest.idEmpresa = idEmpresaInt;
                vendedorRequest.IdVendedor = idVendedor;
            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(lista);
            }



            try
            {
                // Obtener supervisor
                ApplicationDbContext db = new ApplicationDbContext();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


                var idUsuarioActual = User.Identity.GetUserId();

                supervisorRequest.IdUsuario = idUsuarioActual;
                supervisorRequest.IdEmpresa = idEmpresaInt;

                if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
                {
                    Response response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/obtenerSupervisorPorIdUsuario");

                    supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                    vendedorRequest.IdSupervisor = supervisorRequest.IdSupervisor;


                }



                // Carga de combo

                lista = await ApiServicio.ObtenerElementoAsync1<List<VendedorRequest>>(vendedorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarVendedoresPorSupervisor");

                ViewBag.IdVendedor = new SelectList(lista, "IdVendedor", "Nombres");



                // Lista de eventos para cargar la agenda
                var listaEventos = await ApiServicio.ObtenerElementoAsync1<List<EventoRequest>>(vendedorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Agendas/ListarEventosPorVendedor");


                return View(listaEventos);
            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View();
            }
        }



        /*
         ************************************* Métodos para mapa Y Ajax-mapa
         */


        public async Task<ActionResult> MapaIndex(string mensaje)
        {

            InicializarMensaje(mensaje);

            return View();

        }

        public async Task<JsonResult> ListaVendedores()
        {

            var lista = new List<UbicacionPersonaRequest>();

            SupervisorRequest supervisorRequest = new SupervisorRequest();
            VendedorRequest vendedorRequest = new VendedorRequest();

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

        public async Task<JsonResult> ListaClientes()
        {
            var lista = new List<ClienteRequest>();

            SupervisorRequest supervisorRequest = new SupervisorRequest();
            VendedorRequest vendedorRequest = new VendedorRequest();

            int idEmpresaInt = 0; ;

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


                lista = await ApiServicio.ObtenerElementoAsync1<List<ClienteRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarClientesPorSupervisor");


                return Json(lista);

            }
            catch (Exception ex)
            {

                return Json(false);

            }

        }



        public async Task<JsonResult> ListaRutas(int IdVendedor)
        {
            var lista = new List<RutaRequest>();

            VendedorRequest vendedorRequest = new VendedorRequest();

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

                //** agregar el idVendedor al vendedorRequest **
                vendedorRequest.IdVendedor = IdVendedor;



                lista = await ApiServicio.ObtenerElementoAsync1<List<RutaRequest>>(vendedorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/ListarRutaVendedores");


                return Json(lista);

            }
            catch (Exception ex)
            {

                return Json(false);

            }

        }

    }
}