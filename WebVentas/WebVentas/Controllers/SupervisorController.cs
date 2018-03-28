﻿using Microsoft.AspNet.Identity;
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
    public class SupervisorController : Controller
    {


        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }
        public ActionResult MapaCalor()
        {
            return View();
        }
        public async Task<ActionResult> Compromisos()
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

            if (userManager.IsInRole(idUsuarioActual, "Supervisor"))
            {
                Response response = await ApiServicio.InsertarAsync(supervisorRequest,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Vendedores/obtenerSupervisorPorIdUsuario");

                supervisorRequest = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());
                  var userWithClaims = (ClaimsPrincipal)User;
                try
                {
                    var lista = await ApiServicio.ObtenerElementoAsync1<SupervisorRequest>(supervisorRequest, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarVendedoresPorSupervisor2");
                    //var objeto = JsonConvert.DeserializeObject<SupervisorRequest>(lista.ListaVendedores.ToString());
                   ViewBag.IdVendedor = new SelectList(lista.ListaVendedores, "IdVendedor", "Nombres");
                    var vista1 = new SupervisorRequest { FechaInicio = DateTime.Now, FechaFin = DateTime.Now };
                    
                    return View(lista);

                }
                catch (Exception ex)
                {

                    ex.ToString();
                    var vista = new SupervisorRequest { FechaInicio = DateTime.Now, FechaFin = DateTime.Now };
                    return View();
                }

                ;
            }
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Compromisos(SupervisorRequest supervisorRequest)
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

                ViewBag.IdVendedor = new SelectList(lista.ListaVendedores, "IdVendedor", "Nombres");

                return View(lista);
            }
            return View();

        }


        public async Task<JsonResult> ClienteVendor(string idVendedor)
        {

            var userWithClaims = (ClaimsPrincipal)User;
            var idEmpresa = userWithClaims.Claims.First(c => c.Type == Constantes.Empresa).Value;
            var empresaActual = new EmpresaActual { IdEmpresa = Convert.ToInt32(idEmpresa) };
            var user = new SupervisorRequest
            {
                IdVendedor = Convert.ToUInt16(idVendedor),
                IdEmpresa = empresaActual.IdEmpresa

            };
            try
            {
                var respusta = await ApiServicio.ObtenerElementoAsync1<VendedorRequest>(user, new Uri(WebApp.BaseAddress)
                                                              , "api/Vendedores/ListarClientesPorVendedor");
                //var a = respusta.ListaClientes.ToString();
                var listaSalida = JsonConvert.DeserializeObject<List<SupervisorRequest>>(JsonConvert.SerializeObject(respusta.ListaClientes).ToString());
                return Json(listaSalida);

            }
            catch (Exception ex)
            {
                return Json(false);

            }
            

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
            Identificacion = supervisorRequest.Identificacion,Apellidos =supervisorRequest.Apellidos,Nombres = supervisorRequest.Nombres, Estado =1,
            Direccion=supervisorRequest.Direccion,Telefono = supervisorRequest.Telefono, IdEmpresa = empresaActual.IdEmpresa };
            var result = await userManager.CreateAsync(user, "A123345.1a");
            db.SaveChanges();
            userManager.AddToRoles(user.Id, "Supervisor");
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

        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var super = new SupervisorRequest
                {
                    IdSupervisor = Convert.ToInt32( id)
                };
                if (!string.IsNullOrEmpty(id))
                {
                    var response = await ApiServicio.ObtenerElementoAsync(super,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/Supervisor/obtenerSupervisor");
                    response.Resultado = JsonConvert.DeserializeObject<SupervisorRequest>(response.Resultado.ToString());

                    if (response.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(response.Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Edit(string id, SupervisorRequest supervisorRequest)
        {
            Response response = new Response();
            ApplicationDbContext db = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = db.Users.Find(supervisorRequest.IdUsuario);

            user.UserName = supervisorRequest.Correo;
            user.Email = supervisorRequest.Correo;
            user.Identificacion = supervisorRequest.Identificacion;
            user.Apellidos = supervisorRequest.Apellidos;
            user.Nombres = supervisorRequest.Nombres;
            user.Direccion = supervisorRequest.Direccion;
            user.Telefono = supervisorRequest.Telefono;
            var result =  await userManager.UpdateAsync(user);
            db.SaveChanges();
            
            if (result!=null)
            {
                return RedirectToAction("Index");
            }
           
            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            Response response = new Response();
            ApplicationDbContext db = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = db.Users.Find(id);
            user.Estado = 0;
            var result = await userManager.UpdateAsync(user);
            db.SaveChanges();
            if (result != null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        
        public async Task<ActionResult>Quitarvendedor(string id, string idsupervisor)
        {
            try
            {
                var super = new SupervisorRequest
                {
                    IdVendedor = Convert.ToInt32(id)
                    
                };

                if (!string.IsNullOrEmpty(id))
                {
                    var response = await ApiServicio.ObtenerElementoAsync(super,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/Supervisor/Quitarvendedor");

                    if (response.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return RedirectToAction("edit",new { id=idsupervisor});


                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return View();
        }
        public async Task<ActionResult> Asignarvendedor(string id, string idsupervisor)
        {
            try
            {
                var super = new SupervisorRequest
                {
                    IdVendedor = Convert.ToInt32(id),
                    IdSupervisor = Convert.ToInt32(idsupervisor)

                };

                if (!string.IsNullOrEmpty(id))
                {
                    var response = await ApiServicio.ObtenerElementoAsync(super,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/Supervisor/Asignarvendedor");

                    if (response.IsSuccess)
                    {
                        InicializarMensaje(null);
                       return RedirectToAction("edit", new { id = idsupervisor });


                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return View();
        }
    }
    

}