using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace irreplaceable.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            vmHome model = new vmHome();
            return View(model);
        }

       
        //[LoginAuthorize(RoleNo = "Member")]
   


        public ActionResult About()
        {

            return View();
        }


        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

    }
}