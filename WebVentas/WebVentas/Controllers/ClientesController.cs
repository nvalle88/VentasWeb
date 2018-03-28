using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    public class ClientesController : Controller
    {

        public async Task<ActionResult> Index(string mensaje)
        {

            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };

            var lista = await ApiServicio.Listar<ClienteRequest>(empresaActual,new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ListarClientes");
            return View(lista);

        }

        public async Task<ActionResult> Create()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };

            var listaTipoCliente=await ApiServicio.Listar<TipoClienteRequest>(empresaActual,new Uri(WebApp.BaseAddress)
                                                               , "api/Clientes/ObtenerTipoClientePorEmpresa");

            var listaVendedores = await ApiServicio.Listar<VendedorRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Vendedores/ListarVendedores");

            ViewBag.IdTipoCliente = new SelectList(listaTipoCliente, "IdTipoCliente", "Tipo");
            ViewBag.IdVendedor = new SelectList(listaVendedores, "IdVendedor", "Nombres");
            return View();

        }

        public async Task<ActionResult> Edit(int id)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };


            var clienteRequests = new ClienteRequest { IdCliente = id };
            var response = await ApiServicio.ObtenerElementoAsync1<Response>(clienteRequests, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ObtenerCliente");

            if (response.IsSuccess)
            {
                var cliente = JsonConvert.DeserializeObject<ClienteRequest>(response.Resultado.ToString());
                var listaTipoCliente = await ApiServicio.Listar<TipoClienteRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Clientes/ObtenerTipoClientePorEmpresa");

                var listaVendedores = await ApiServicio.Listar<VendedorRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                                   , "api/Vendedores/ListarVendedores");

                ViewBag.IdTipoCliente = new SelectList(listaTipoCliente, "IdTipoCliente", "Tipo", cliente.IdTipoCliente);
                ViewBag.IdVendedor = new SelectList(listaVendedores, "IdVendedor", "Nombres", cliente.IdVendedor);
                return View(cliente);
            }

            return View();
        }


        [HttpPost]
        public async Task<JsonResult> EditarCliente(string idcliente,string flatitud, string flongitud, string nombre, string direccion, string apellido, string telefono, string telefonomovil, string identificacion, string email, string tipocliente, string vendedor)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;


            var cliente = new ClienteRequest
            {
                IdCliente=Convert.ToInt32(idcliente),
                Identificacion = identificacion,
                IdEmpresa = Convert.ToInt32(idEmpresa),
            };

            var respuesta = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                  , "api/Clientes/ExisteClienteEditarPorEmpresa");

            if (respuesta == null)
            {
                return Json("Error");
            }

            if (respuesta.IsSuccess == true)
            {
                return Json("Existe");
            }

            var clienteEditar = new ClienteRequest
            {
                Identificacion = identificacion,
                IdEmpresa = Convert.ToInt32(idEmpresa),
                Apellido = apellido,
                Direccion = direccion,
                Email = email,
                IdTipoCliente = Convert.ToInt32(tipocliente),
                IdVendedor = Convert.ToInt32(vendedor),
                Latitud = Convert.ToDouble(flatitud),
                Longitud = Convert.ToDouble(flongitud),
                Nombre = nombre,
                Telefono = telefono,
                TelefonoMovil = telefonomovil,
            };

            var respuestaInsertar = await ApiServicio.EditarAsync<Response>(clienteEditar, new Uri(WebApp.BaseAddress)
                                                             , "api/Clientes/EditarCliente");

            if (respuestaInsertar.IsSuccess)
            {
                return Json(true);
            }

            return Json(false);

        }

        [HttpPost]
        public async Task<JsonResult> CreateCliente(string razonSocial, string flatitud,string flongitud,string nombre,string direccion, string apellido,string telefono,string telefonomovil, string identificacion,string email,string tipocliente, string vendedor)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;


            var cliente = new ClienteRequest
            {
                Identificacion = identificacion,
                IdEmpresa = Convert.ToInt32(idEmpresa),
            };

        var respuesta = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                              , "api/Clientes/ExisteClientePorEmpresa");

            if (respuesta==null)
            {
                return Json("Error");
            }

            if (respuesta.IsSuccess==true)
            {
                return Json("Existe");
            }

            var clienteInsertar = new ClienteRequest
            {
                Identificacion = identificacion,
                IdEmpresa = Convert.ToInt32(idEmpresa),
                Apellido=apellido,
                Direccion=direccion,
                Email=email,
                IdTipoCliente=Convert.ToInt32(tipocliente),
                IdVendedor=Convert.ToInt32(vendedor),
                Latitud=Convert.ToDouble(flatitud),
                Longitud=Convert.ToDouble(flongitud),
                Nombre=nombre,
                Telefono=telefono,
                TelefonoMovil=telefonomovil,
                RazonSocial=razonSocial,
            };

            var respuestaInsertar = await ApiServicio.InsertarAsync<Response>(clienteInsertar, new Uri(WebApp.BaseAddress)
                                                             , "api/Clientes/InsertarCliente");

            if (respuestaInsertar.IsSuccess)
            {
                return Json(true);
            }

            return  Json( false);

        }
    }
}