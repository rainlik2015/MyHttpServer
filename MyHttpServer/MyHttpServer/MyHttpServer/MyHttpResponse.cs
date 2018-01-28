using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MyHttpServer.Common;
using System.Collections.Specialized;

namespace MyHttpServer
{
    public class MyHttpResponse
    {
        private WebHeaderCollection _Headers = new WebHeaderCollection();
        public WebHeaderCollection Headers
        {
            set
            {
                _Headers = value;
            }
            get
            {
                return _Headers;
            }
        }

        public long ContentLength64{set;get;}
                    

        public Stream OutputStream { set; get; }

        public int StatusCode { set; get; }

        public string StatusDescription { set; get; }

        public bool SendChunked { set; get; }

        public void AddHeader(string k, string v)
        {
            if (Headers != null)
                Headers.Add(k, v);
        }

        public MyHttpResponse()
        {
        }

        /// <summary>
        /// 输出响应报文流
        /// </summary>
        /// <param name="response"></param>
        /// <param name="httpBodyStream"></param>
        /// <param name="enableGzip"></param>
        /// <returns></returns>
        public int ResponseToBrowser(Stream httpBodyStream, bool enableGzip = false)
        {
            if (httpBodyStream == null)
            {
                ContentLength64 = 0;
                ResponseBytes(null);
                return 0;
            }
            else
            {
                if (enableGzip)
                {
                    Headers[HttpResponseHeader.ContentEncoding] = "gzip";
                }
                byte[] buf = httpBodyStream.ReadAllBytes();
                if (enableGzip)
                {
                    buf = Gzip.GzipBytes(buf);
                }
                ContentLength64 = buf.Length;//统一设置报文长度
                ResponseBytes(buf);
                return buf.Length;
            }
        }

        /// <summary>
        /// 向HTTP响应流输出字节序列（不负责设置响应报文标头，不负责关闭响应流）
        /// </summary>
        /// <param name="response"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public int ResponseBytes(byte[] buf)
        {
            int len = 0;
            if (buf != null)
                len = buf.Length;
            OutputStream.Write(buf, 0, len);
            return len;
        }

        /// <summary>
        /// 向HTTP响应流输出字符串（不负责设置响应报文标头，不负责关闭响应流）
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseString"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public int ResponseString(string responseString, Encoding enc = null)
        {
            if (enc == null)
                enc = System.Text.Encoding.UTF8;
            var ms = responseString.ToStream(enc);
            return ResponseToBrowser(ms);
        }

        public int SendBytes(Stream stream, bool enableGzip = false)
        {
            return ResponseToBrowser(stream, enableGzip);
        }
        
        public void Close()
        {
 
        }
    }
}