using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
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

        
        public async Task<ActionResult> VendedorIndex(string mensaje)
        {
            List<VendedorRequest> lista = new List<VendedorRequest>();
            VendedorRequest vr = new VendedorRequest();
            Response response = new Response();

            InicializarMensaje(mensaje);


            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;


                vr.idEmpresa = Convert.ToInt32( idEmpresa );
            }
            catch (Exception ex) {

                InicializarMensaje(Mensaje.ErrorIdEmpresa);
                return View(lista);
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
                guardar = true; ///RRRRRRRRRRRRRRRRRRRRRR borrar este guardar, solo esta porque aun no hay mi rol

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

                    InstanciaUsuario.Estado = 0; // revisar !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


                    db.Entry(InstanciaUsuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("VendedorIndex", new { mensaje = Mensaje.BorradoSatisfactorio });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("VendedorIndex", new { mensaje = Mensaje.Error });
                }
            }
        }


    }
}