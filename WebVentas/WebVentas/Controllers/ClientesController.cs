using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    [Authorize( Roles ="Supervisor,GerenteGeneral")]
    public class ClientesController : Controller
    {

        public async Task<ActionResult> Index(int? idEstado)
        {

            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual();
            if (idEstado == null)
            {
                empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa), IdEstado = EstadoCliente.Activo };
                ViewBag.IdEstadoCliente = new SelectList(ListaClientesEstados.ListaEstados, "IdEstado", "Nombre", EstadoCliente.Activo);
            }
            else
            {
                empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa), IdEstado = Convert.ToInt32(idEstado) };
                ViewBag.IdEstadoCliente = new SelectList(ListaClientesEstados.ListaEstados, "IdEstado", "Nombre", Convert.ToInt32(idEstado));
            }


            var lista = await ApiServicio.Listar<ClienteRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ListarClientes");


            return View(lista);

        }
        [HttpPost]
        public async Task<ActionResult> Index(string IdEstadoCliente, string s)
        {

            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa), IdEstado = Convert.ToInt32(IdEstadoCliente) };

            var lista = await ApiServicio.Listar<ClienteRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ListarClientes");

            ViewBag.IdEstadoCliente = new SelectList(ListaClientesEstados.ListaEstados, "IdEstado", "Nombre", Convert.ToInt32(IdEstadoCliente));
            return View(lista);

        }

        public async Task<ActionResult> Desactivar(int id, int idEstado)
        {

            var cliente = new ClienteRequest { IdCliente = id };

            var response = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/DesactivarCliente");

            if (response.IsSuccess)
            {
                return RedirectToAction("Index", new { idEstado });
            }

            return null;

        }

        public async Task<ActionResult> Activar(int id, int idEstado)
        {

            var cliente = new ClienteRequest { IdCliente = id };

            var response = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ActivarCliente");
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", new { idEstado });
            }

            return null;

        }

        private async Task CargarCombos()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };

            var listaTipoCliente = await ApiServicio.Listar<TipoClienteRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Clientes/ObtenerTipoClientePorEmpresa");

            var listaVendedores = await ApiServicio.Listar<VendedorRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Vendedores/ListarVendedores");

            ViewBag.IdTipoCliente = new SelectList(listaTipoCliente, "IdTipoCliente", "Tipo");
            ViewBag.IdVendedor = new SelectList(listaVendedores, "IdVendedor", "Nombres");
        }

        private async Task CargarCombos(ClienteRequest clienteRequest)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };

            var listaTipoCliente = await ApiServicio.Listar<TipoClienteRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Clientes/ObtenerTipoClientePorEmpresa");

            var listaVendedores = await ApiServicio.Listar<VendedorRequest>(empresaActual, new Uri(WebApp.BaseAddress)
                                                               , "api/Vendedores/ListarVendedores");

            ViewBag.IdTipoCliente = new SelectList(listaTipoCliente, "IdTipoCliente", "Tipo",clienteRequest.IdTipoCliente);
            ViewBag.IdVendedor = new SelectList(listaVendedores, "IdVendedor", "Nombres",clienteRequest.IdVendedor);
        }


        public async Task<ActionResult> Create()
        {
            var cliente = new ClienteRequest();
            await CargarCombos();
            return View(cliente);

        }


        public async Task<ActionResult> PerfilCliente(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index");
            };
            var cliente = new ClienteRequest
            {
                IdCliente = Convert.ToInt32(id),
            };
            var respuesta = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                 , "api/Clientes/ObtenerCliente");

            var clienteRequest = JsonConvert.DeserializeObject<ClienteRequest>(respuesta.Resultado.ToString());

            var foto = string.IsNullOrEmpty(clienteRequest.Foto) != true ? clienteRequest.Foto.Replace("~", WebApp.BaseAddress) : "";
            clienteRequest.Foto = foto;
            var firma = string.IsNullOrEmpty(clienteRequest.Firma) != true ? clienteRequest.Firma.Replace("~", WebApp.BaseAddress) : ""; ;
            clienteRequest.Firma = firma;
            return View(clienteRequest);

        }

        private string NormalizarFoto(ClienteRequest cliente)
        {
            var foto = string.IsNullOrEmpty(cliente.Foto) != true ? cliente.Foto.Replace("~", WebApp.BaseAddress) : "";

            return foto;
        }

        private string NormalizarFirma(ClienteRequest cliente)
        {
            var firma = string.IsNullOrEmpty(cliente.Firma) != true ? cliente.Firma.Replace("~", WebApp.BaseAddress) : ""; ;
            return firma;
           
        }

        public async Task<ActionResult> Edit(int id)
        {
           


            var clienteRequests = new ClienteRequest { IdCliente = id };
            var response = await ApiServicio.ObtenerElementoAsync1<Response>(clienteRequests, new Uri(WebApp.BaseAddress)
                                                                , "api/Clientes/ObtenerCliente");

            if (response.IsSuccess)
            {
                var cliente = JsonConvert.DeserializeObject<ClienteRequest>(response.Resultado.ToString());

                await CargarCombos(cliente);

                cliente.Foto = NormalizarFoto(cliente);
                cliente.Firma = NormalizarFirma(cliente);

                return View(cliente);
            }

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Edit(HttpPostedFileBase fileUpload, string Latitud, string Longitud, ClienteRequest clienteRequest)
        {
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
                clienteRequest.Latitud = Convert.ToDouble(Latitud);
                clienteRequest.Longitud = Convert.ToDouble(Longitud);

                var cliente = new ClienteRequest
                {
                    IdCliente = Convert.ToInt32(clienteRequest.IdCliente),
                    Identificacion = clienteRequest.Identificacion,
                    IdEmpresa = Convert.ToInt32(idEmpresa),
                };

                var respuesta = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                      , "api/Clientes/ExisteClienteEditarPorEmpresa");

                if (respuesta == null)
                {
                    ModelState.AddModelError("", "Ha ocurrido un error al conectarse al servicio...");
                    await CargarCombos();
                    return View(clienteRequest);
                }

                if (respuesta.IsSuccess == true)
                {
                    ModelState.AddModelError("Identificacion", "El cliente ya existe...");

                    await CargarCombos(clienteRequest);
                    clienteRequest.Foto = NormalizarFoto(clienteRequest);
                    clienteRequest.Firma = NormalizarFirma(clienteRequest);
                    return View(clienteRequest);
                }

                cliente.Apellido = clienteRequest.Apellido;
                cliente.Direccion = clienteRequest.Direccion;
                cliente.Email = clienteRequest.Email;
                cliente.Identificacion = clienteRequest.Identificacion;
                cliente.IdTipoCliente = Convert.ToInt32(clienteRequest.IdTipoCliente);
                cliente.IdVendedor = Convert.ToInt32(clienteRequest.IdVendedor);
                cliente.Latitud = Convert.ToDouble(clienteRequest.Latitud);
                cliente.Longitud = Convert.ToDouble(clienteRequest.Longitud);
                cliente.Nombre = clienteRequest.Nombre;
                cliente.Telefono = clienteRequest.Telefono;
                cliente.TelefonoMovil = clienteRequest.TelefonoMovil;
                cliente.RazonSocial = clienteRequest.RazonSocial;

                var respuestaInsertar = await ApiServicio.EditarAsync<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                 , "api/Clientes/EditarCliente");

                if (respuestaInsertar.IsSuccess)
                {

                    if (fileUpload != null)
                    {
                        var fichero = readFileContents(fileUpload);
                        var foto = new ArchivoRequest { Id = Convert.ToString(clienteRequest.IdCliente), Array = fichero, Tipo = 1 };


                        var fotoRequest = await ApiServicio.InsertarAsync<Response>(foto, new Uri(WebApp.BaseAddress)
                                                                        , "Api/Archivos/Insertar");

                        if (fotoRequest.IsSuccess)
                        {
                            var clienteActualizarFoto = new ClienteRequest
                            {
                                IdCliente = clienteRequest.IdCliente,
                                Foto = fotoRequest.Resultado.ToString(),
                            };
                            var fotoActualizar = await ApiServicio.InsertarAsync<Response>(clienteActualizarFoto, new Uri(WebApp.BaseAddress)
                                                                        , "api/Clientes/EditarFotoCliente");

                            if (fotoActualizar.IsSuccess)
                            {
                                return RedirectToAction("Index", clienteRequest.Estado);
                            }
                        }
                    }

                    return RedirectToAction("Index", clienteRequest.Estado);
                }

                return RedirectToAction("Index", clienteRequest.Estado);
            }
            catch (Exception)
            {

                throw;
            }

        }


        private byte[] readFileContents(HttpPostedFileBase file)
        {
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();

            //////using (MemoryStream ms = new MemoryStream())
            //////{
            ////// file.InputStream.CopyTo(ms);
            ////// fileBytes = ms.GetBuffer();
            //////}

            return fileBytes;
        }

        [HttpPost]
        public async Task<ActionResult> Create(HttpPostedFileBase fileUpload, string Latitud, string Longitud, ClienteRequest clienteRequest)
        {
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
                clienteRequest.Latitud = Convert.ToDouble(Latitud);
                clienteRequest.Longitud = Convert.ToDouble(Longitud);

                var cliente = new ClienteRequest
                {
                    Identificacion = clienteRequest.Identificacion,
                    IdEmpresa = Convert.ToInt32(idEmpresa),
                };

                var respuesta = await ApiServicio.ObtenerElementoAsync1<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                      , "api/Clientes/ExisteClientePorEmpresa");


                if (respuesta == null)
                {
                    ModelState.AddModelError("", "Ha ocurrido un error al conectarse al servicio...");
                    await CargarCombos();
                    return View(clienteRequest);
                }

                if (respuesta.IsSuccess == true)
                {
                    ModelState.AddModelError("Identificacion", "El cliente ya existe...");

                    await CargarCombos();
                    clienteRequest.Identificacion = "";
                    return View(clienteRequest);
                }


                cliente.Apellido = clienteRequest.Apellido;
                cliente.Direccion = clienteRequest.Direccion;
                cliente.Email = clienteRequest.Email;
                cliente.IdTipoCliente = Convert.ToInt32(clienteRequest.IdTipoCliente);
                cliente.IdVendedor = Convert.ToInt32(clienteRequest.IdVendedor);
                cliente.Latitud = Convert.ToDouble(clienteRequest.Latitud);
                cliente.Longitud = Convert.ToDouble(clienteRequest.Longitud);
                cliente.Nombre = clienteRequest.Nombre;
                cliente.Telefono = clienteRequest.Telefono;
                cliente.TelefonoMovil = clienteRequest.TelefonoMovil;
                cliente.RazonSocial = clienteRequest.RazonSocial;
                cliente.Estado = EstadoCliente.Activo;


                var respuestaInsertar = await ApiServicio.InsertarAsync<Response>(cliente, new Uri(WebApp.BaseAddress)
                                                                 , "api/Clientes/InsertarCliente");


                if (respuestaInsertar.IsSuccess)
                {

                    if (fileUpload != null)
                    {
                        var clienteSalvado = JsonConvert.DeserializeObject<ClienteRequest>(respuestaInsertar.Resultado.ToString());
                        var fichero = readFileContents(fileUpload);
                        var foto = new ArchivoRequest { Id = Convert.ToString(clienteSalvado.IdCliente), Array = fichero, Tipo = 1 };


                        var fotoRequest = await ApiServicio.InsertarAsync<Response>(foto, new Uri(WebApp.BaseAddress)
                                                                        , "Api/Archivos/Insertar");

                        if (fotoRequest.IsSuccess)
                        {
                            var clienteActualizarFoto = new ClienteRequest
                            {
                                IdCliente = clienteSalvado.IdCliente,
                                Foto = fotoRequest.Resultado.ToString(),
                            };
                            var fotoActualizar = await ApiServicio.InsertarAsync<Response>(clienteActualizarFoto, new Uri(WebApp.BaseAddress)
                                                                        , "api/Clientes/EditarFotoCliente");

                            if (fotoActualizar.IsSuccess)
                            {
                                return RedirectToAction("Index", EstadoCliente.Activo);
                            }
                        }
                    }



                    return RedirectToAction("Index", EstadoCliente.Activo);
                }


                return RedirectToAction("Index", EstadoCliente.Activo);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}