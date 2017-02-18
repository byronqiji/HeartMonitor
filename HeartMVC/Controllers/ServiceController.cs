using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HeartMVC.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        public ActionResult Index()
        {
            HeartMonitor.HeartServerDirMonitor.Single.RefreshDir();
            return View();
        }
       
	}
}