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
using System.Web;
using System.Web.Http;


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

        public HttpResponseMessage Post([FromBody]JToken jsonbody)
        {
            AccountController accountController = new AccountController();

            dynamic data = JObject.Parse(jsonbody.ToString());

            string userId = data.userid;
            string userPassword = data.password;

            var result = SignInManager.PasswordSignIn(userId, userPassword, false, false);

            if (result.Equals(SignInStatus.Success))
            {
                User.Identity.GetUserId();
                return new HttpResponseMessage(HttpStatusCode.Accepted);

            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
