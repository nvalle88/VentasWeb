﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using WebVentas.ObjectModel;
using WebVentas.Services;
using WebVentas.Utils;

namespace WebVentas.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public int IdEmpresa { get; set; }


        public string TokenContrasena { get; set; }
        public string Foto { get; set; }
        public int Estado { get; set; }
        public string Direccion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           
            var userRequest = new UserRequest { Id = userIdentity.GetUserId() };
            var response = await ApiServicio.ObtenerElementoAsync1<Response>(userRequest, new Uri(WebApp.BaseAddress)
                                                                , "api/Empresa/ObtenerEmpresaPorCliente");

            if (response.IsSuccess)
            {
                var empresa = JsonConvert.DeserializeObject<AspNetUsers>(response.Resultado.ToString());
                userIdentity.AddClaim(new Claim(Constantes.Empresa, Convert.ToString(empresa.IdEmpresa)));
            }

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}