using HeartModel;
using System;
using LogHelper;

namespace HeartModel
{
    /// <summary>
    /// AppDomain辅助类
    /// </summary>
    public class AppDomainHelper
    {
        /// <summary>
        /// 心跳类名
        /// </summary>
        public const string HeartClassName = "HeartServer";

        /// <summary>
        /// 创建新的AppDomain
        /// </summary>
        /// <param name="dirPath">AppDomain的执行文件夹</param>
        /// <param name="appDomainName"></param>
        /// <returns></returns>
        public static AppDomain CreateHeartServerAppDomain(string dirPath, string appDomainName)  //string filePath, 
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = dirPath;
            setup.ConfigurationFile = "app.config";
            setup.ApplicationName = appDomainName;

            AppDomain heartDomain = null;

            try
            {
                heartDomain = AppDomain.CreateDomain(appDomainName, null, setup);
            }
            catch (ArgumentNullException anex)
            {
                LogServer.WriteException("CreateDomain", anex, dirPath);
                if (heartDomain != null)
                    AppDomain.Unload(heartDomain);

                heartDomain = null;
            }

            return heartDomain;
        }

        /// <summary>
        /// 创建一个心跳代理类
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static HeartBase CreateHeart(AppDomain domain, string assemblyName, string className)
        {
            HeartBase heart = null;
            try
            {
                heart = domain.CreateInstanceAndUnwrap(assemblyName, className) as HeartBase;
            }
            catch
            {
                heart = null;
            }

            return heart;
        }
    }
}
