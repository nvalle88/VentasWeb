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


                InicializarMensaje("");
                return View(vendedor);
            }
            catch
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(vendedor);
            }
        }


        public async Task<ActionResult> Create(string mensaje)
        {
            VendedorRequest vendedorRequest = new VendedorRequest();

            InicializarMensaje(mensaje);

            return View(vendedorRequest);
        }

        

        [HttpPost]
        public async Task<ActionResult> Create(VendedorRequest vendedorRequest)
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

            try
            {

                ApplicationDbContext db = new ApplicationDbContext();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                

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

                    //supervisorRequest.IdEmpresa = Convert.ToInt32(idEmpresa);
                    vendedorRequest.IdUsuario = user.Id;

                    response = await ApiServicio.InsertarAsync(vendedorRequest,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Vendedores/InsertarVendedor");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("VendedorIndex");
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
        


    }
}