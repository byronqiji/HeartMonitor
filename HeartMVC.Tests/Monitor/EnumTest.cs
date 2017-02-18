using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeartModel.StateMachine;
using CommonUtiliy;

namespace HeartMVC.Tests.Monitor
{
    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void EnumDescriptionTest()
        {
            int x = 1;
            HeartServerState hss = (HeartServerState)x;

            string y = "NotLoaded";
            HeartServerState hss1 = (HeartServerState)Enum.Parse(typeof(HeartServerState), y);

            string z = hss.ToString();

            string des = hss.GetDescription();
            //Assert.AreEqual("未加载", ((HeartServerState)x).GetDescription());


        }
    }
}
