using System;
using System.Reflection;

namespace CommonUtiliy
{
    public class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public EnumDescriptionAttribute(string description)
        {
            this.description = description;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return description; }
            set { description = value; }
        }
    }

    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;

            object[] attrs = fd.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            string name = string.Empty;
            foreach (EnumDescriptionAttribute attr in attrs)
            {
                name = attr.Remark;
            }
            return name;
        }
    }
}
