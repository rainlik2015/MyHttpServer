using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace MyHttpServer
{
    public class SocketStream : NetworkStream
    {
        private MyHttpRequest request = null;
        private MyHttpResponse response = null;

        public SocketStream(Socket sock, MyHttpRequest req,MyHttpResponse resp)
            : base(sock)
        {
            request = req;
            response = resp;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Socket == null)
                return;
            SendHeaders();

            if (buffer == null || buffer.Length == 0)
                return;
            Socket.Send(buffer);
            Close();
        }
        private void SendHeaders()
        {
            StringBuilder sb = new StringBuilder();

            response.Headers.Add("Content-Length", response.ContentLength64.ToString());

            sb.AppendLine(string.Format("HTTP/{0} {1} {2}", request.ProtocolVersion.ToString(), response.StatusCode, response.StatusDescription));

            foreach (var k in response.Headers)
            {
                sb.AppendLine(string.Format("{0}: {1}", k, response.Headers[k.ToString()]));
            }
            sb.AppendLine();
            var headersString = sb.ToString();
            var buf = Encoding.Default.GetBytes(headersString);
            Socket.Send(buf);
        }
        public override void Close()
        {
            if (Socket != null)
                Socket.Close();
            base.Close();
        }
        private int SendBytes(Stream stream, bool enableGzip = false)
        {
            if (Socket == null)
                return 0;
            int r = 0;
            stream.ReadAllByStep(buf =>
            {
                Socket.Send(buf);
                r += buf.Length;
            });
            return r;
        }
    }
}
