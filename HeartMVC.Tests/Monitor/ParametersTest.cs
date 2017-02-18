using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeartModel;
using HeartModel.StateMachine;
using HeartMonitor;
using System.Runtime.CompilerServices;
using System.IO;

[assembly: InternalsVisibleTo("HeartModel.StateMachine.RunningHeart.IsDo")]
namespace HeartMVC.Tests.Monitor
{
    [TestClass]
    public class ParametersTest
    {
        [TestMethod]
        public void InitialParametersTest()
        {
            ParametersHandler.LoadParametersConfig();

            int i = 0;
            foreach (var item in ParameterPool.Single)
            {
                ++i;
            }
            Assert.AreEqual(4, i);

            Assert.AreEqual(4, ParameterPool.Single.Count);

            Assert.AreEqual("60000", ParameterPool.Single["MonitorSleepTime"].Value);
        }

        [TestMethod]
        public void ReloadParemters_Test()
        {
            ParametersHandler.LoadParametersConfig();
            ParametersHandler.ReloadParameterConfig();

            int i = 0;
            foreach (var item in ParameterPool.Single)
            {
                ++i;
            }
            Assert.AreEqual(4, i);

            Assert.AreEqual(4, ParameterPool.Single.Count);

            Assert.AreEqual("60000", ParameterPool.Single["MonitorSleepTime"].Value);
        }

        [TestMethod]
        public void HeartMonitorTest()
        {
            ParametersHandler.LoadParametersConfig();

            HeartServerDirMonitor.Single.RefreshDir();

            Assert.AreEqual(1, HeartServerDirMonitor.Single.Count);

            HeartServerInfo hsi = HeartServerDirMonitor.Single["Heart_Monitor_Test"];

            hsi.Load();
            hsi.Run();

            System.Threading.Thread.Sleep(1000 * 10);

            //hsi.Pause();

            //System.Threading.Thread.Sleep(1000 * 10);

            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Exception, hsi.State);
            //Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Exception, hsi.State);

            hsi.Unload();

            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.NotLoaded, hsi.State);
        }

        [TestMethod]
        public void HeartMonitorTest2()
        {
            ParametersHandler.LoadParametersConfig();

            HeartServerDirMonitor.Single.RefreshDir();

            Assert.AreEqual(1, HeartServerDirMonitor.Single.Count);

            HeartServerInfo hsi = HeartServerDirMonitor.Single["Heart_Monitor_Test"];

            TimeSpan span = DateTime.Now.TimeOfDay;
            hsi.SpanInfo = new TimeConfig() {
                StartTime = span,
                EndTime = span,
                Span = new TimeSpan(0, 0, 30)
            };

            hsi.Load();
            hsi.Run();
            System.Threading.Thread.Sleep(1);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);
            hsi.Pause();
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);

            System.Threading.Thread.Sleep(1000 * 12);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Loaded, hsi.State);
        }

        [TestMethod]
        public void HeartMonitorTest3()
        {
            ParametersHandler.LoadParametersConfig();

            HeartServerDirMonitor.Single.RefreshDir();

            Assert.AreEqual(1, HeartServerDirMonitor.Single.Count);

            HeartServerInfo hsi = HeartServerDirMonitor.Single["Heart_Monitor_Test"];

            TimeSpan span = DateTime.Now.TimeOfDay;
            hsi.SpanInfo = new TimeConfig()
            {
                StartTime = span,
                EndTime = span,
                Span = new TimeSpan(0, 0, 30)
            };

            hsi.Load();
            hsi.Run();
            System.Threading.Thread.Sleep(1);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);
            hsi.Pause();
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);

            System.Threading.Thread.Sleep(1000 * 12);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Loaded, hsi.State);

            System.Threading.Thread.Sleep(1);
            hsi.Run();
            System.Threading.Thread.Sleep(1);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);

            System.Threading.Thread.Sleep(1000 * 90);
            hsi.Pause();
            System.Threading.Thread.Sleep(1000 * 3);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Loaded, hsi.State);

            hsi.Unload();
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.NotLoaded, hsi.State);
        }

        [TestMethod]
        public void RefreshTest()
        {
            ParametersHandler.LoadParametersConfig();

            HeartServerDirMonitor.Single.RefreshDir();
            HeartServerInfo hsi = HeartServerDirMonitor.Single["Heart_Monitor_est"];

            hsi.Load();
            hsi.Run();

            System.Threading.Thread.Sleep(1000 * 10);
            ParametersHandler.LoadParametersConfig();
            for (int i = 0; i < 10; ++i)
            {
                HeartServerDirMonitor.Single.RefreshDir();
                System.Threading.Thread.Sleep(1000);
            }
            hsi = HeartServerDirMonitor.Single["Heart_Monitor_Test"];

            Assert.AreEqual(HeartModel.StateMachine.HeartServerState.Running, hsi.State);

            LogHelper.LogServer.WriteException("Test", new Exception("Over test"));
        }

        [TestMethod]
        public void IsDo_Test()
        {
            DateTime now = DateTime.Now;

            // 当前时间在区间段内
            HeartServerInfo hsi = new HeartServerInfo()
            {
                SpanInfo = new TimeConfig()
                {
                    StartTime = now.AddHours(-1).TimeOfDay,
                    EndTime = now.AddHours(1).TimeOfDay
                }
            };
            RunningHeart r = new RunningHeart(hsi);
            Assert.AreEqual(true, r.IsDo());

            //时间相同
            hsi.SpanInfo.StartTime = now.AddHours(1).TimeOfDay;
            hsi.SpanInfo.EndTime = now.AddHours(1).TimeOfDay;
            Assert.AreEqual(true, r.IsDo());

            // 当前时间在开始区间以外
            hsi.SpanInfo.StartTime = now.AddHours(1).TimeOfDay;
            hsi.SpanInfo.EndTime = now.AddHours(2).TimeOfDay;
            Assert.AreEqual(false, r.IsDo());

            // 当前时间在结束时间以外
            hsi.SpanInfo.StartTime = now.AddHours(-1).TimeOfDay;
            hsi.SpanInfo.EndTime = now.AddHours(-2).TimeOfDay;
            Assert.AreEqual(false, r.IsDo());
        }

        [TestMethod]
        public void LoadXml()
        {
            TimeConfig tc = new TimeConfig();
            tc.StartTime = System.DateTime.Now.TimeOfDay;
            tc.EndTime = System.DateTime.Now.TimeOfDay;

            string xml = CommonUtiliy.SerializationHelper<TimeConfig>.Serialize(tc);
        }

        [TestMethod]
        public void SerializeTimeConfigMapTest()
        {
            string key = "aaa";
            TimeConfig tc = new TimeConfig();
            DateTime now = DateTime.Now;
            tc.StartTime = now.AddHours(-1).TimeOfDay;
            tc.EndTime = now.AddHours(1).TimeOfDay;

            TimeConfigCollection.Single.Add(key, tc);

            tc.StartTime = now.AddHours(-2).TimeOfDay;

            TimeConfigCollection.Single.Add("dddd", tc);
        }

        [TestMethod]
        public void DeserializeTimeConfigMapTest()
        {
            Assert.AreEqual(2, TimeConfigCollection.Single.Count);
        }

        [TestMethod]
        public void SynchronizeTimeConfigTest()
        {
            if (File.Exists(TimeConfigCollection.Single.configFilePath))
                File.Delete(TimeConfigCollection.Single.configFilePath);

            Assert.AreEqual(false, File.Exists(TimeConfigCollection.Single.configFilePath));

            TimeConfigCollection.Single.Clear();
            Assert.AreEqual(0, TimeConfigCollection.Single.Count);

            HeartServerDirMonitor.Single.RefreshDir();

            HeartServerInfo hsi = null;
            foreach (KeyValuePair<string, HeartServerInfo> item in HeartServerDirMonitor.Single)
            {
                hsi = item.Value;
                break;
            }

            DateTime now = DateTime.Now;
            TimeConfig tc = new TimeConfig() 
            {
                StartTime = now.AddHours(-1).TimeOfDay,
                EndTime = now.AddHours(1).TimeOfDay
            };

            if (hsi != null)
                hsi.SpanInfo = tc;

            Assert.AreEqual(true, File.Exists(TimeConfigCollection.Single.configFilePath));

            Assert.AreEqual(1, TimeConfigCollection.Single.Count);

            Assert.AreEqual(tc, TimeConfigCollection.Single[hsi.Name]);
        }
    }
}
