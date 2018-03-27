using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            WebApp.BaseAddress= System.Configuration.ConfigurationManager.AppSettings["ServiciosVentas"];
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
