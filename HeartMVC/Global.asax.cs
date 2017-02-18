using HeartMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HeartMVC
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

            ParametersHandler.LoadParametersConfig();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            LogHelper.LogServer.WriteException("Global", HttpContext.Current.Server.GetLastError());
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
