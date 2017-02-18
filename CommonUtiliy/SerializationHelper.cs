using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CommonUtiliy
{
    public class SerializationHelper<T>
    {
        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();
        private static XmlSerializer GetSerializer()
        {
            int type_hash = typeof(T).GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(typeof(T)));

            return serializer_dict[type_hash];
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Deserialize(string content)
        {
            try
            {
                using (Stream sm = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sm);
                }    
            }
            catch (Exception ex)
            {               
                throw ex;
            }
        }



        /// <summary>
        /// Xml序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(T obj)
        {
            string returnStr = "";

            XmlSerializer serializer = GetSerializer();
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }
            return returnStr;

        }


    }
}
