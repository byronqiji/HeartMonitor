using System;
using System.Reflection;
using System.Xml;

namespace HeartMonitor
{
    public class Parameter
    {
        protected const string AttributeKey = "key";

        protected const string NodeName = "name";
        protected const string NodeValue = "value";
        protected const string NodeDescription = "description";
        protected const string NodeType = "type";

        public Parameter(XmlNode pNode)
        {  
            if (pNode.Attributes.Count == 1 && pNode.Attributes[AttributeKey] != null)
            {
                this.Key = pNode.Attributes[AttributeKey].Value;
                foreach (XmlNode sub in pNode.ChildNodes)
                {
                    switch (sub.Name)
                    {
                        case NodeName:
                            this.Name = sub.InnerText;
                            break;
                        case NodeValue:
                            this.Value = sub.InnerText;
                            break;
                        case NodeDescription:
                            this.Description = sub.InnerText;
                            break;
                        case NodeType:
                            this.ValueType = sub.InnerText;
                            break;
                    }
                }
            }
        }

        public string Key { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 参数描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参数值类型
        /// </summary>
        public string ValueType { get; set; }
    }
}
