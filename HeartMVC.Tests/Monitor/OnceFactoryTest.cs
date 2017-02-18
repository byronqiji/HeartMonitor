using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeartMonitor;
using HeartModel.StateMachine;
using CommonUtiliy;
using System.IO;

namespace HeartMVC.Tests.Monitor
{
    [TestClass]
    public class OnceFactoryTest
    {
        [TestMethod]
        public void CreateOnceServerTest()
        {
            ParametersHandler.LoadParametersConfig();

            HeartServerDirMonitor.Single.RefreshDir();

            Assert.AreEqual(1, HeartServerDirMonitor.Single.Count);

            OnceServer os = OnceServerFactory.CreateOnceServerByKey("Heart_Monitor_Test");

            os.Do(System.DateTime.Now.AddHours(-1), System.DateTime.Now.AddHours(1));
        }
    }
}
