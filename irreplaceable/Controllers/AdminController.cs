using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace irreplaceable.Controllers
{
    public class AdminController : Controller
    {

        [LoginAuthorize(RoleNo = "Admin,User")]
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("", "首頁", "", 12);
            return View();
        }


        public ActionResult PrgOpen(string id)
        {
            using (tblPrograms programs = new tblPrograms())
            {
                SessionService.ColumnSort = null;
                programs.PrgOpen(id);
                if (SessionService.PrgSecuritys.IsSecurity)
                    return RedirectToAction(SessionService.ActionName, SessionService.ControllerName);
                return RedirectToAction("Index", "Admin");
            }
        }
    }


   
}