using HeartMVC.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeartMVC.Controllers
{
    public class HelperController : Controller
    {
        //
        // GET: /Helper/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpAction(string Name, string Action)
        {
            S_Json_Base json = new S_Json_Base();
            json.Status = 0;
            try
            {
                HeartModel.StateMachine.HeartServerInfo model = HeartMonitor.HeartServerDirMonitor.Single[Name];
                switch (Action)
                {
                    case "run":
                        model.Run();
                        break;
                    case "load":
                        model.Load();
                        break;
                    case "unload":
                        model.Unload();
                        break;
                    case "pause":
                        model.Pause();
                        break;
                    default:
                        break;
                }
                json.Status = 1;
                json.Message = "执行成功";
            }
            catch (Exception ex)
            {

                json.Message = ex.Message;
            }
            return this.Json(json);
        }

        public ActionResult GetConfig(string Key)
        {
            TimeConfigModel model =new TimeConfigModel();
            model.Key = Key;
            HeartModel.TimeConfig configModel = HeartMonitor.TimeConfigCollection.Single[Key];
            if (configModel != null)
            {
                string spanH = configModel.Span.Hours < 10 ? ("0" + configModel.Span.Hours) : configModel.Span.Hours.ToString();
                string spanM = configModel.Span.Minutes < 10 ? ("0" + configModel.Span.Minutes) : configModel.Span.Minutes.ToString();
                string spanS = configModel.Span.Seconds < 10 ? ("0" + configModel.Span.Seconds) : configModel.Span.Seconds.ToString();

                string stHours = configModel.StartTime.Hours < 10 ? ("0" + configModel.StartTime.Hours) : configModel.StartTime.Hours.ToString();
                string stMins = configModel.StartTime.Minutes < 10 ? ("0" + configModel.StartTime.Minutes) : configModel.StartTime.Minutes.ToString();

                string etHours = configModel.EndTime.Hours < 10 ? ("0" + configModel.EndTime.Hours) : configModel.EndTime.Hours.ToString();
                string etMins = configModel.EndTime.Minutes < 10 ? ("0" + configModel.EndTime.Minutes) : configModel.EndTime.Minutes.ToString();

                model.SpanTime = spanH + ":" + spanM + ":" + spanS;
                model.StartTime = stHours + ":" + stMins;
                model.EndTime = etHours + ":" + etMins;
            }
            return this.Json(model);          
        }

        public ActionResult SaveConfig(string Key,string SpanTime, string StartTime,string EndTime)
        {
            S_Json_Base json = new S_Json_Base();

            SpanTime = SpanTime.Replace('：', ':').Trim();
            StartTime = StartTime.Replace('：', ':').Trim();
            EndTime = EndTime.Replace('：', ':').Trim();
           
            try
            {
                HeartModel.TimeConfig timeModel = new HeartModel.TimeConfig();
                timeModel.Key = Key;
                timeModel.Span = Convert.ToDateTime(SpanTime).TimeOfDay;
                timeModel.StartTime = Convert.ToDateTime(StartTime).TimeOfDay;
                timeModel.EndTime = Convert.ToDateTime(EndTime).TimeOfDay;
                HeartModel.StateMachine.HeartServerInfo ServerModel = HeartMonitor.HeartServerDirMonitor.Single[Key];
                ServerModel.SpanInfo = timeModel;
                json.Status = 1;
                json.Message = "配置成功";
            }
            catch (Exception ex)
            {
               json.Status =0;
               json.Message = ex.Message;
            }

            
            return this.Json(json);
        }


       
	}
}