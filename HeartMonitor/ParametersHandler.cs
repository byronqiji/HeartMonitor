using System.Configuration;
using System.Xml;

namespace HeartMonitor
{
    public class ParametersHandler : ConfigurationSection
    {
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            ParameterPool.ParametersReader = reader;
        }

        /// <summary>
        /// 第一次加载参数配置
        /// </summary>
        public static void LoadParametersConfig()
        {
            ParametersHandler ph = (ParametersHandler)System.Configuration.ConfigurationManager.GetSection("monitorParameters/parameters");
        }

        /// <summary>
        /// 重新加载参数配置
        /// </summary>
        public static void ReloadParameterConfig()
        {
            ParameterPool.ReloadParameters();
            LoadParametersConfig();
        }
    }
}
