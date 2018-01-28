using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer.Log
{
    /// <summary>
    /// 日志输出到控制台
    /// </summary>
    public class ConsoleLog : BaseLog
    {
        public ConsoleLog(LOGLEVEL level)
            : base(level)
        { }

        protected override void InternalDoLog(string str)
        {
            Console.WriteLine(str);
        }
    }
}
