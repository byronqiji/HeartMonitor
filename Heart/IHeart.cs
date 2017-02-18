using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJGTHeart
{
    public enum HeartState
    {
        Loaded = 1,
        Running = 2,
        Stop = 3,
    }

    public interface IHeart
    {
        /// <summary>
        /// 心跳执行主体方法
        /// </summary>
        void DoAction();

        /// <summary>
        /// 心跳描述，用于UI呈现
        /// </summary>
        /// <returns>心跳描述信息</returns>
        string DescripteDll();

        /// <summary>
        /// 开启心跳
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭心跳
        /// </summary>
        void Close();

        HeartState State { get; set; }

        string HeartName { get; set; }
    }
}
