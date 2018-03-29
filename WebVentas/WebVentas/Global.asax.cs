using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebVentas.Models;
using WebVentas.Utils;

namespace WebVentas
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           

            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            WebApp.BaseAddress= System.Configuration.ConfigurationManager.AppSettings["ServiciosVentas"];

            ListaClientesEstados.ListaEstados = new List<EstadoClienteNombre>();

            ListaClientesEstados.ListaEstados.Add(new EstadoClienteNombre
            {
                IdEstado = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteActivado"]),
                Nombre = "Activos",
            });

            ListaClientesEstados.ListaEstados.Add(new EstadoClienteNombre
            {
                IdEstado = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteDesactivado"]),
                Nombre = "Inactivos",
            });
            ListaClientesEstados.ListaEstados.Add(new EstadoClienteNombre

            {
                IdEstado = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteTodos"]),
                Nombre = "Todos",
            });

            EstadoCliente.Activo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteActivado"]);
            EstadoCliente.Inactivo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteDesactivado"]);
            EstadoCliente.Todos = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClienteTodos"]);

        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (!roleManager.RoleExists("Vendedor"))
            {
                roleManager.Create(new IdentityRole("Vendedor"));
            }

            if (!roleManager.RoleExists("Supervisor"))
            {
                roleManager.Create(new IdentityRole("Supervisor"));
            }

            if (!roleManager.RoleExists("GerenteGeneral"))
            {
                roleManager.Create(new IdentityRole("GerenteComercial"));
            }

        }
    }
}
