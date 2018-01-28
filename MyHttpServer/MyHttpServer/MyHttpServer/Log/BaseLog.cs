using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer.Log
{
    /// <summary>
    /// 日志类
    /// </summary>
    public abstract class BaseLog
    {
        protected LOGLEVEL logLevel;
        public BaseLog(LOGLEVEL level)
        {
            logLevel = level;
        }
        private void DoLog(string str, LOGLEVEL level)
        {
            if (level >= logLevel)
            {
                InternalDoLog(string.Format("【{0}】{1}", DateTime.Now, str));
            }
        }
        protected abstract void InternalDoLog(string str);

        public void Debug(string str)
        {
            DoLog(str, LOGLEVEL.DEBUG);
        }
        public void Info(string str)
        {
            DoLog(str, LOGLEVEL.INFO);
        }
        public void Warn(string str)
        {
            DoLog(str, LOGLEVEL.WARN);
        }
        public void Error(string str)
        {
            DoLog(str, LOGLEVEL.ERROR);
        }
        public void Fatal(string str)
        {
            DoLog(str, LOGLEVEL.FATAL);
        }
    }
}
