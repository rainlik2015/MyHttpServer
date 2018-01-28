using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MyHttpServer.Listener
{
    /// <summary>
    /// 使用内置的HttpListener来监听
    /// </summary>
    public class BuildInHttpListener : IListener
    {
        private HttpListener httpListener = null;

        public int Port { set; get; }

        public BuildInHttpListener(int port)
        {
            Port = port;
            httpListener = new HttpListener();
            httpListener.Prefixes.Clear();
            httpListener.Prefixes.Add(string.Format(@"http://localhost:{0}/", Port));
        }

        public void StartListen()
        {
            httpListener.Start();
        }

        public MyListenerContext GetContext()
        {
            MyListenerContext r = new MyListenerContext();

            var buildInContext = httpListener.GetContext();

            var req = buildInContext.Request;
            r.Request = new MyHttpRequest
            {
                ContentEncoding = req.ContentEncoding,
                ContentLength64 = req.ContentLength64,
                ContentType = req.ContentType,
                HasEntityBody = req.HasEntityBody,
                Headers = req.Headers,
                HttpMethod = req.HttpMethod,
                InputStream = req.InputStream,
                RemoteEndPoint = req.RemoteEndPoint,
                Url = req.Url,
                UserAgent = req.UserAgent,
                ProtocolVersion = req.ProtocolVersion
            };

            var resp = buildInContext.Response;
            r.Response = new MyHttpResponse
            {
                ContentLength64 = resp.ContentLength64,
                Headers = resp.Headers,
                OutputStream = resp.OutputStream,
                SendChunked = resp.SendChunked,
                StatusCode = resp.StatusCode,
                StatusDescription = resp.StatusDescription
            };
            
            return r;
        }

        public bool IsListening
        {
            get { return httpListener.IsListening; }
        }

        public void StopListen()
        {
            httpListener.Stop();
        }


        public bool IsSupported
        {
            get
            {
                return HttpListener.IsSupported;
            }
        }
    }
}
