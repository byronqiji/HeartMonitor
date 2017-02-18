using HeartMonitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HeartMVC.Controllers
{
    public class MonitorController : Controller
    {
        //
        // GET: /Monitor/
        public ActionResult Index()
        {
            return View(ParameterPool.Single);
        }


        public ActionResult GetModel(string key)
        {
            Parameter model = ParameterPool.Single[key] as Parameter;
            return this.Json(model);
        }

        public ActionResult Save(string Key, string Name, string Value, string Description)
        {

            Parameter entity = ParameterPool.Single[Key] as Parameter;

            S_Json_Base json = new S_Json_Base();
            json.Status = 1;
            json.Message = "保存成功";
            return this.Json(json);
        }
	}
}