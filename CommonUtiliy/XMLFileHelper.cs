using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace CommonUtiliy
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public class XMLFileHelper
    {
        /// <summary>
        /// 获取XML文件内容
        /// </summary>
        /// <param name="filepath">相对文件路径</param>
        /// <returns>返回XML文件内容</returns>
        public static string LoadXmlFile(string filepath)
        {
            if (filepath.EndsWith("\\"))
                filepath = System.AppDomain.CurrentDomain.BaseDirectory +"\\"+ filepath;
            else
                filepath = System.AppDomain.CurrentDomain.BaseDirectory +"\\"+ filepath;

            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            return doc.OuterXml;
        }
    }
}
