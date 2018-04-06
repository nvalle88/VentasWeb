using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebVentas.Utils;

namespace WebVentas.Controllers.Api
{
    [RoutePrefix("api/webapi")]
    public class WebApiController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public WebApiController() { }

        public WebApiController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/WebApi
        public IEnumerable Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        public Response Post([FromBody]JToken jsonbody)
        {
            AccountController accountController = new AccountController();

            dynamic data = JObject.Parse(jsonbody.ToString());

            string userId = data.userid;
            string userPassword = data.password;

            var result = SignInManager.PasswordSignIn(userId, userPassword, false, false);

            if (result.Equals(SignInStatus.Success))
            {
                return new Response
                {
                    Message = "LoginOk",
                    IsSuccess = true,
                    Resultado = User.Identity.GetUserId()
                };

            }
            else
            {
                return new Response
                {
                    Message = "Incorrecto",
                    IsSuccess = false,
                    Resultado = false,
                };
               
            }
        }

        [HttpPost]
        [Route("CambiarContraseña")]
        public async Task<Response> CambiarContraseña([FromBody]JToken jsonbody)
        {

            //if (recuperarContrasenaRequest.Contrasena != recuperarContrasenaRequest.ConfirmarContrasena)
            //{
            //    ModelState.AddModelError("", "La contraseña y la confirmación no coinciden...");
            //}
            //var user = await UserManager.FindByEmailAsync(recuperarContrasenaRequest.Email);
            //await UserManager.RemovePasswordAsync(user.Id);
            //var pass = await UserManager.AddPasswordAsync(user.Id, recuperarContrasenaRequest.ConfirmarContrasena);
            //if (!pass.Succeeded)
            //{
            //    ModelState.AddModelError("", "La contraseña no cumple con el formato establecido ...");
            //    return View(recuperarContrasenaRequest);
            //}

            //return RedirectToAction("Login");
            return null;
        }

    }
}
