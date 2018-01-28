using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MyHttpServer.Cache;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// 解析URL并路由到适当的处理程序
    /// </summary>
    public abstract class BaseUrlHandler
    {
        private ICache cache;

        /// <summary>
        /// 网站文件的物理路径
        /// </summary>
        public string PhysicalPath { set; get; }

        /// <summary>
        /// 网站项目的名称
        /// </summary>
        public string WebSiteName { set; get; }

        public BaseUrlHandler(string siteName, string path, ICache c = null)
        {
            WebSiteName = siteName;
            PhysicalPath = path;
            cache = c;
        }

        /// <summary>
        /// 能否解析当前URL
        /// </summary>
        /// <returns></returns>
        public abstract bool CanHandle(MyHttpRequest req);

        /// <summary>
        /// 根据HTTP请求，返回响应报文正文字节序列（不用缓存）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public abstract Stream GetHttpBodyWithoutCache(MyHttpRequest req, MyHttpResponse resp);

        /// <summary>
        /// 获取响应报文的正文
        /// 虚方法，默认使用缓存，子类可以重写为不使用缓存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual Stream GetHttpBodyStream(MyHttpRequest req, MyHttpResponse resp)
        {
            return GetHttpBodyWithCache(req,resp);
        }
        
        /// <summary>
        /// 设置响应报文头（如果需要的话）
        /// 虚方法，子类可以重写
        /// </summary>
        /// <param name="response"></param>
        public virtual void PrepareResponseHeaders(MyHttpResponse response) 
        { }

        /// <summary>
        /// 向浏览器发送响应报文
        /// </summary>
        /// <param name="req"></param>
        /// <param name="response"></param>
        public void SendResponse(MyHttpRequest req, MyHttpResponse response)
        {
            PrepareResponseHeaders(response);
            response.SendChunked = true;//当报文长度不确定时用这个【TransferEncoding=chunked】
            response.Headers[HttpResponseHeader.Server] = "MyServer";//统一设置标头
            response.Headers[HttpResponseHeader.LastModified] = DateTime.Now.ToString();//统一设置LastModified
            
            Stream httpBodyStream = GetHttpBodyStream(req,response);
            if (httpBodyStream == null)
            {
                response.ContentLength64 = 0;
                ResponseWithGzipCheck(req, response, httpBodyStream);
            }
            else
            {
                if (cacheShoot)
                {
                    response.AddHeader("IsCacheShoot", "Yes");
                }
                ResponseWithGzipCheck(req, response, httpBodyStream);
                httpBodyStream.Close();
                httpBodyStream.Dispose();
            }
            response.OutputStream.Close();//统一关闭响应的输出流
            response.Close();//统一关闭响应流
        }

        /// <summary>
        /// 考虑是否启用服务器端GZip压缩
        /// </summary>
        /// <param name="req"></param>
        /// <param name="response"></param>
        /// <param name="httpBodyStream"></param>
        protected void ResponseWithGzipCheck(MyHttpRequest req, MyHttpResponse response, Stream httpBodyStream)
        {
            bool useGzip = false;
            if (req.Headers["Accept-Encoding"].ToLower().Contains("gzip"))
                useGzip = true;
            response.ResponseToBrowser(httpBodyStream, useGzip);
        }
        private static bool cacheShoot = false;
        /// <summary>
        /// 根据HTTP请求，返回响应报文字节序列（用缓存）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        protected Stream GetHttpBodyWithCache(MyHttpRequest req, MyHttpResponse resp)
        {
            Stream r = null;
            
            var ifModified = req.Headers["If-Modified-Since"];
            //如果浏览器发送过来的请求中没有If-Modified-Since标头，则不读缓存，重新生成页面，并更新缓存
            if (ifModified == null || ifModified == "")
            {
                cacheShoot = false;
                r = GetHttpBodyWithoutCache(req,resp);
                //更新缓存
                cache.AddOrUpdateCache(req.Url.ToString(), r, DateTime.Now, 100);
                return r;
            }

            var ifModifiedSince = DateTime.Parse(ifModified);
            var cachedHtmlFileBytes = cache.GetCache(req.Url.ToString(), ifModifiedSince);
            if (cachedHtmlFileBytes == null)//缓存未命中
            {
                cacheShoot = false;
                r = GetHttpBodyWithoutCache(req,resp);
                //写入缓存
                cache.AddOrUpdateCache(req.Url.ToString(), r, DateTime.Now, 100);
            }
            else//缓存命中
            {
                cacheShoot = true;
                r = cachedHtmlFileBytes;
            }
            return r;
        }
    }
}
