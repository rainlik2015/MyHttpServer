using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Specialized;

namespace MyHttpServer.Listener
{
    /// <summary>
    /// 监听本地端口的抽象接口
    /// </summary>
    public interface IListener
    {
        int Port { set; get; }
        bool IsListening { get; }
        bool IsSupported { get; }

        void StartListen();
        void StopListen();
        MyListenerContext GetContext();
    }
}
