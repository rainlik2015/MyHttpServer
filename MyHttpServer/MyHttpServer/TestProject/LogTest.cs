using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyHttpServer.Log;
using System.IO;

namespace TestProject
{
    [TestClass]
    public class LogTest
    {
        private const string MSG = "This is an error!";

        [TestMethod]
        public void TestFileLog()
        {
            string logFile = @"D:\\testx.txt";
            BaseLog log = new FileLog(logFile, LOGLEVEL.INFO);
            log.Error(MSG);
            string txt = File.ReadAllText(logFile, Encoding.Default);
            Assert.IsNotNull(txt);
        }

        [TestMethod]
        public void TestDelegateLog()
        {
            BaseLog log = new DelegateLog(LOGLEVEL.INFO, arg =>
            {
                Assert.AreEqual(arg, MSG);
            });
            log.Error(MSG);
        }
    }
}
