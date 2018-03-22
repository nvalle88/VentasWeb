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
            var fname = userWithClaims.Claims.First(c => c.Type == "IdEmpresa");
            
            var lista = await ApiServicio.Listar<ClienteRequest>(new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ListarClientes");
            return View(lista);

        }

        public async Task<ActionResult> Create()
        {
            //var listaTipoCliente= ApiServicio.Listar<ClienteRequest>(new Uri(WebApp.BaseAddress)
            //                                                    , "api/Clientes/ListarClientes");

            //ViewBag.IdEdades = new SelectList(db.Edades, "IdEdades", "Descripcion");
            //ViewBag.IdEdades = new SelectList(db.Edades, "IdEdades", "Descripcion");
            return View();

        }

    }
}