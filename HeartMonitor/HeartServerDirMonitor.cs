using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;

using LogHelper;
using HeartModel;
using HeartModel.StateMachine;
using System.Runtime.ExceptionServices;

namespace HeartMonitor
{
    enum RuleEnum
    { 
        CheckFileName = 1,
        CheckAssemblyClassName = 2,
    }

    /// <summary>
    /// 心跳服务文件夹监控类  单例模式   
    /// 心跳服务程序集文件名必须与程序集命名空间名一致(包括大小写一致)
    /// 心跳服务程序集文件名必须以 Heart_Monitor_ 开头，只加载.dll文件格式
    /// 心跳服务必须包含继承于HeartModel.HeartBase的HeartServer类名; 若命名空间名为 Heart_Monitor_  则类名的完全限定名为 Heart_Monitor_.HeartServer
    /// 心跳服务拥有自己的配资文件app.config，于心跳程序集文件Heart_Monitor_XXX.dll处于同一个文件夹下
    /// 一个心跳服务创建一个AppDomain，若加载失败，或不符合规则，卸载创建的AppDomain，并删除对应的文件夹
    /// </summary>
    public class HeartServerDirMonitor : IEnumerable
    {
        private const string HeartAssemblyNameStart = "Heart_Monitor_";

        private HeartBase tempHear = null;

        /// <summary>
        /// 心跳服务程序集规则
        /// </summary>
        private Dictionary<RuleEnum, Func<AppDomain, string, bool>> ruleMap;

        /// <summary>
        /// 单例实体
        /// </summary>
        private static HeartServerDirMonitor monitor;

        /// <summary>
        /// 有效的心跳服务列表
        /// </summary>
        private ConcurrentDictionary<string, HeartServerInfo> effectiveHeartServerMap;

        private HeartServerDirMonitor()
        {
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ruleMap = new Dictionary<RuleEnum, Func<AppDomain, string, bool>>();
            ruleMap.Add(RuleEnum.CheckFileName, CheckFileName);
            ruleMap.Add(RuleEnum.CheckAssemblyClassName, CheckAssemblyClassName);

            effectiveHeartServerMap = new ConcurrentDictionary<string, HeartServerInfo>();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                HeartServerInfo hsInfo = this[ex.Source];
                if (hsInfo != null)
                {
                    hsInfo.ReceiveException(ex);
                }
                else
                {
                    LogServer.WriteException("Default AppDomain", ex);

                    Exception subException = ex.InnerException;
                    while (subException != null)
                    {
                        LogServer.WriteException("InnerException", subException);
                        subException = subException.InnerException;
                    }
                }
            }
        }

        void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            HeartServerInfo hsInfo = this[e.Exception.Source];
            if (hsInfo != null)
            {
                hsInfo.ReceiveException(e.Exception);
            }
            else
            {
                LogServer.WriteException("Default AppDomain  First", e.Exception);

                Exception subException = e.Exception.InnerException;
                while (subException != null)
                {
                    LogServer.WriteException("InnerException First", subException);
                    subException = subException.InnerException;
                }
            }
        }

        /// <summary>
        /// 监控单例
        /// </summary>
        public static HeartServerDirMonitor Single
        {
            get
            {
                if (monitor != null)
                    return monitor;

                HeartServerDirMonitor temp = new HeartServerDirMonitor();
                Interlocked.CompareExchange(ref monitor, temp, null);

                return monitor;
            }
        }

        /// <summary>
        /// 监控文件夹,若文件夹加载失败，删除这个文件夹，若已加载  跳过该文件夹
        /// </summary>
        public void RefreshDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            if (path.EndsWith("\\"))
                path += ParameterPool.Single[ParameterName.DirPath].Value + "\\";
            else
                path += "\\" + ParameterPool.Single[ParameterName.DirPath].Value + "\\";

            bool b = false;

            if (Directory.Exists(path))
            {
                string[] dirList = Directory.GetDirectories(path);

                for (int i = 0; i < dirList.Length; ++i)
                {
                    b = false;
                    string[] fileList = Directory.GetFiles(dirList[i]);

                    for (int j = 0; j < fileList.Length; ++j)
                    {
                        b = FilterHeartAssembly(fileList[j], dirList[i]);

                        if (b)
                            break;
                    }

                    if (!b)
                    {
                        // 文件夹中没有可以加载的心跳服务程序集，删除该文件夹
                        DeleteHeartDir("文件夹中没有以" + HeartAssemblyNameStart + "开头的.dll文件", dirList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 检测程序集文件命名规则
        /// 以Heart_Monitor_开头，后缀名为dll
        /// </summary>
        /// <param name="domian"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool CheckFileName(AppDomain domian, string fileName)
        {
            return fileName.StartsWith(HeartAssemblyNameStart) && fileName.EndsWith(".dll");
        }
        
        /// <summary>
        /// 检测程序集是否包含指定的类名HeartServer，并继承于 HeartBase
        /// 程序集文件名与命名空间名保持一致(包括大小写) 文件名为: Heart_Monitor_XXX，则类名的完全限定名为: Heart_Monitor_XXX.HeartServer
        /// </summary>
        /// <param name="domian"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool CheckAssemblyClassName(AppDomain domian, string fileName)
        {
            try
            {
                tempHear = domian.CreateInstanceAndUnwrap(fileName, string.Format("{0}.{1}", fileName, AppDomainHelper.HeartClassName)) as HeartBase;
            }
            catch
            {
                tempHear = null;
            }

            return tempHear != null;
        }

        /// <summary>
        /// 删除文件夹 并记录相关信息
        /// </summary>
        /// <param name="errorInfo">删除文件夹的原因</param>
        /// <param name="dirPath"></param>
        private void DeleteHeartDir(string errorInfo, string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
                LogServer.WriteInfoLog(string.Format("删除心跳文件夹{0}错误:{1}{0}路径:{2}", Environment.NewLine, errorInfo, dirPath));
            }
        }

        /// <summary>
        /// 创建心跳AppDomain
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        private bool FilterHeartAssembly(string filePath, string dirPath)
        {
            bool b = false;
            string fileName = filePath.Substring(dirPath.Length + 1);
            if (ruleMap[RuleEnum.CheckFileName](null, fileName)) // 检验.dll文件名规则是否以 Heart_Monitor_ 开头
            {
                fileName = fileName.Split('.')[0];
                b = effectiveHeartServerMap.ContainsKey(fileName);

                if (!b)
                {
                    AppDomain heartDomain = AppDomainHelper.CreateHeartServerAppDomain(dirPath, fileName);  //filePath, 

                    if (heartDomain != null)
                    {
                        try
                        {
                            if (!ruleMap[RuleEnum.CheckAssemblyClassName](heartDomain, fileName))
                            {
                                DeleteHeartDir("加载的程序集没有继承于HeartBase的HeartServer对象", dirPath);
                            }
                            else
                            {
                                HeartServerInfo info = new HeartServerInfo();
                                info.AssemblyFilePath = filePath;
                                info.DirPath = dirPath;
                                info.Name = fileName;
                                info.Description = tempHear.GetDescription();
                                info.SpanInfo = TimeConfigCollection.Single[fileName];
                                info.OnTimeConfigChange += info_OnTimeConfigChange;

                                effectiveHeartServerMap.TryAdd(fileName, info);
                            }

                            // 验证完后  不管是否与规则相符 都执行卸载AppDomain操作
                            AppDomain.Unload(heartDomain);
                            heartDomain = null;

                            b = true;
                        }
                        catch (FileLoadException flex)
                        {
                            LogServer.WriteException("Load dll", flex, filePath);
                            AppDomain.Unload(heartDomain);
                            heartDomain = null;

                            DeleteHeartDir("加载的程序集没有继承于HeartBase的HeartServer对象", dirPath);
                        }
                    }
                }
            }

            return b;
        }

        void info_OnTimeConfigChange(string key, TimeConfig tc)
        {
            if (TimeConfigCollection.Single[key] == null)
                TimeConfigCollection.Single.Add(key, tc);
            else
                TimeConfigCollection.Single[key] = tc;
        }

        private void SynchronizateTimeConfig(string key)
        {
            TimeConfig tc = TimeConfigCollection.Single[key];
            if (tc != null)
                effectiveHeartServerMap[key].SpanInfo = tc;
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return effectiveHeartServerMap.GetEnumerator();
        }

        /// <summary>
        /// 有效的心跳服务数量
        /// </summary>
        public int Count
        {
            get
            {
                if (effectiveHeartServerMap == null)
                    return 0;
                else
                    return effectiveHeartServerMap.Count;
            }
        }

        /// <summary>
        /// 获取key value值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HeartServerInfo this[string key]
        {
            get
            {
                if (effectiveHeartServerMap.ContainsKey(key))
                    return effectiveHeartServerMap[key];
                else
                    return null;
            }
        }
    }
}
