using HeartModel.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommonUtiliy;
using HeartModel;

namespace HeartMonitor
{
    public class OnceServerFactory
    {
        internal static OnceServer CreateOnceServerByKey(string key)
        {
            OnceServer os = null;

            HeartServerInfo hsi = HeartServerDirMonitor.Single[key];
            if (hsi != null)
            {
                DirectoryHelper.DirectoryCopy(hsi.DirPath, OnceDirPath + hsi.Name);
                os = new OnceServer();

                os.Domain = AppDomainHelper.CreateHeartServerAppDomain(OnceDirPath + hsi.Name, hsi.Name);

                if (os.Domain != null)
                    os.HeartServer = AppDomainHelper.CreateHeart(os.Domain, hsi.Name, hsi.Name + "." + AppDomainHelper.HeartClassName);
            }
            
            return os;
        }

        private static string OnceDirPath
        {
            get 
            {
                string temp = AppDomain.CurrentDomain.BaseDirectory;
                if (!temp.EndsWith("\\"))
                    temp += "\\";

                temp += ParameterPool.Single[ParameterName.OnceDirPath].Value;

                if (!temp.EndsWith("\\"))
                    temp += "\\";

                return temp;
            }
        }
    }
}
