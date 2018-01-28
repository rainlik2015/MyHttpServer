using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer.Log
{
    public class DelegateLog : BaseLog
    {
        private Action<string> xAction;
        public DelegateLog(LOGLEVEL level, Action<string> act)
            : base(level)
        {
            xAction = act;
        }
        protected override void InternalDoLog(string str)
        {
            if (xAction != null)
                xAction(str);
        }
    }
}