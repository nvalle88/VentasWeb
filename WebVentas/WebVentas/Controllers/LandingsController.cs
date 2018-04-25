using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebVentas.Controllers
{
    public class LandingsController : Controller
    {
        // GET: Landings
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login()
        {          
            return RedirectToAction("Login");
        }
    }
}