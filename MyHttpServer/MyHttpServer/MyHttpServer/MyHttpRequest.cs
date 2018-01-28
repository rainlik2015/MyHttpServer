using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace MyHttpServer
{
    public class MyHttpRequest
    {
        public Uri Url { set; get; }

        private NameValueCollection _Headers = new NameValueCollection();
        public NameValueCollection Headers
        {
            get
            {
                return _Headers;
            }
            set
            {
                _Headers = value;
            }
        }

        public Stream InputStream { set; get; }
        public Encoding ContentEncoding { set; get; }
        public long ContentLength64 { set; get; }
        public string ContentType { set; get; }
        public string HttpMethod { set; get; }
        public bool HasEntityBody { set; get; }
        public IPEndPoint RemoteEndPoint { set; get; }
        public string UserAgent { set; get; }
        public Version ProtocolVersion { set; get; }
    }
}
