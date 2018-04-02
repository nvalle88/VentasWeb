using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VentaServicios.Utils;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Controllers
{
    public class InformacionGerencialController : Controller
    {
        // GET: InformesGerenciales
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
        #region MapaCalor
        public async Task<ActionResult> MapaCalor()
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
            MapaCalorRequest mapacalor = new MapaCalorRequest();

            mapacalor.IdEmpresa = idEmpresaInt;

            var lista = await ApiServicio.ObtenerElementoAsync1<MapaCalorRequest>(mapacalor, new Uri(WebApp.BaseAddress)
                                                      , "api/MapaCalor/ObtenerTipoClienteTipoCompromisoPorEmpresa");
            ViewBag.idTipoCliente = new SelectList(lista.ListaTipoCliente, "idTipoCliente", "Tipo");
            ViewBag.IdTipoCompromiso = new SelectList(lista.ListaTipoCompromiso, "IdTipoCompromiso", "Descripcion");
            return View();
        }

        public async Task<JsonResult> PuntosTipoCliente(int idTipoCliente)
        {
            try
            {
                MapaCalorRequest mapacalor = new MapaCalorRequest();

                mapacalor.IdTipoCLiente = idTipoCliente;
                var respusta = await ApiServicio.ObtenerElementoAsync1<MapaCalorRequest>(mapacalor, new Uri(WebApp.BaseAddress)
                                                          , "api/MapaCalor/ListarClientesPorTipoCliente");
                //var a = respusta.ListaClientes.ToString();
                var listaSalida = JsonConvert.DeserializeObject<List<ClienteRequest>>(JsonConvert.SerializeObject(respusta.ListaClientes).ToString());
                return Json(listaSalida);

            }
            catch (Exception ex)
            {
                return Json(false);

            }


        }

        #endregion
    }
}