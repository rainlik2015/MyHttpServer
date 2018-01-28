using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer.Log
{
    /// <summary>
    /// 日志输出到文件
    /// </summary>
    public class FileLog : BaseLog
    {
        private string logFile;
        public FileLog(string path, LOGLEVEL level)
            : base(level)
        {
            logFile = path;
        }
        protected override void InternalDoLog(string str)
        {
            File.AppendAllLines(logFile, new string[] { str });
        }
    }
}