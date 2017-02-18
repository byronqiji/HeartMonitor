using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonUtiliy;

namespace HeartMVC.Tests
{
    [TestClass]
    public class CommonUtilityTest
    {
        [TestMethod]
        public void CopyDirectoryTest()
        {
            DirectoryHelper.DirectoryCopy(@"E:\Debug", 
                @"E:\Work\Code\理财网二期\HeartForTest\HeartServerTest\HeartServerTest\dll");
        }
    }
}
