using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyHttpServer.Listener
{
    /// <summary>
    /// 使用Socket来监听
    /// </summary>
    public class SocketListener : IListener
    {
        private Socket listenSocket = null;
        private Socket acceptedSocket = null;

        public event Action NewConnectionComingEvent;

        public int Port { set; get; }

        public SocketListener(int port)
        {
            Port = port;
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress hostIP = IPAddress.Any;
            IPEndPoint ep = new IPEndPoint(hostIP, Port);
            listenSocket.Bind(ep);
        }

        public void StartListen()
        {
            listenSocket.Listen(100);
            isListening = true;
        }

        private MyHttpRequest GetHttpRequest()
        {
            MyHttpRequest result = new MyHttpRequest();
            if (acceptedSocket == null)
                return null;
            var bytes = acceptedSocket.ReceiveConveniently();
            var requestString = Encoding.ASCII.GetString(bytes);
            var lines = requestString.Split('\r');

            #region 设置Headers
            foreach (var line in lines.Skip(1))
            {
                string temp = line.Trim();
                if (temp == null || temp == "")
                    continue;
                var kvString = temp.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (kvString.Length == 0)
                    continue;
                result.Headers.Add(kvString[0], kvString[1]);
            }
            #endregion

            #region 设置HttpMethod、Url、ProtocolVersion
            var httpMethodLine = lines[0].Trim();
            var httpMethodLines = httpMethodLine.Split(' ');
            if (httpMethodLines.Length == 3)
            {
                result.HttpMethod = httpMethodLines[0];
                string hostAndPort = result.Headers["Host"];
                var strs = hostAndPort.Split(':');
                UriBuilder b = new UriBuilder("http", strs[0], int.Parse(strs[1]));
                var baseUri = b.Uri;
                var uri = new Uri(baseUri, httpMethodLines[1]);
                result.Url = uri;
                try
                {
                    result.ProtocolVersion = new Version(httpMethodLines[2].Replace("HTTP/", ""));
                }
                catch
                { }
            }
            #endregion

                

            result.RemoteEndPoint = acceptedSocket.RemoteEndPoint as IPEndPoint;

            string body = lines.Last().Trim();
            result.HasEntityBody = body != "";

            #region 设置请求输入流
            Stream ms = new MemoryStream();
            if (body != "")
            {
                var buf = Encoding.ASCII.GetBytes(body);
                ms.Write(buf, 0, buf.Length);
                result.InputStream = ms;
            }
            else
            {
                ms = MemoryStream.Null;
            }
            #endregion

            result.UserAgent = result.Headers["User-Agent"];

            return result;
        }

        public MyListenerContext GetContext()
        {
            System.Threading.Thread.Sleep(500);//为什么必须延迟一会才行？
            
            acceptedSocket = listenSocket.Accept();
            if (NewConnectionComingEvent != null)
                NewConnectionComingEvent();

            MyListenerContext r = new MyListenerContext();

            var req = GetHttpRequest();
            r.Request = req;

            var resp = new MyHttpResponse();
            if (acceptedSocket != null)
                resp.OutputStream = new SocketStream(acceptedSocket, req, resp);
            r.Response = resp;

            return r;
        }
        private bool isListening = false;

        public bool IsListening
        {
            get
            {
                return isListening;
            }
        }

        public void StopListen()
        {
            listenSocket.Close();
            isListening = false;
        }


        public bool IsSupported
        {
            get
            {
                return true;
            }
        }
    }
}
