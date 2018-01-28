using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using MyHttpServer.UrlRoute;
using System.Threading;
using MyHttpServer.Common;
using MyHttpServer.Log;
using MyHttpServer.Cache;
using MyHttpServer.Listener;

namespace MyHttpServer
{
    /// <summary>
    /// HTTP服务器核心类
    /// </summary>
    public class HttpServer
    {
        private BaseUrlHandler urlHandler;
        private bool isServerRunning = false;
        private IListener listener = null;
        private BaseLog Logger { set; get; }
        private WebSiteConfig _Config = null;
        public WebSiteConfig Config
        {
            get
            {
                return _Config;
            }
        }
        private ICache cache = null;
        public HttpServer(WebSiteConfig cfg,BaseLog log)
        {
            _Config = cfg;
            Logger = log;

            _Config.PhysicalPath = _Config.PhysicalPath == null ?
      Directory.GetCurrentDirectory() :
      ((_Config.PhysicalPath.EndsWith(@"\") ? _Config.PhysicalPath : _Config.PhysicalPath + @"\"));

            listener = new BuildInHttpListener(int.Parse(Config.Port));
            //listener = new SocketListener(int.Parse(Config.Port));

            //cache = new LocalFileCache(_Config.PhysicalPath);
            cache = new RedisCache();
            urlHandler = new HandlerChain(_Config.SiteName, _Config.PhysicalPath, cache);
        }
        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="isSyn">是否以同步模式开启</param>
        public void StartServer(bool isSyn = true)
        {
            //若服务器已经开启，则不用再重复开启
            if (isServerRunning)
            {
                Logger.Warn("Server is already running!");
                return;
            }

            System.Threading.ThreadPool.QueueUserWorkItem(obj =>
            {
                if(!listener.IsSupported)
                {
                    Logger.Fatal("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }
                if (!listener.IsListening)
                {
                    try
                    {
                        listener.StartListen();
                    }
                    catch (Exception e)
                    {
                        Logger.Fatal(string.Format("{0},端口号{1}已经被占用！", e.Message, Config.Port));
                        return;
                    }
                }
                isServerRunning = true;

                Logger.Info(string.Format("Listening port[{0}] ...", Config.Port));
                Logger.Info(Environment.NewLine);

                while (isServerRunning)
                {
                    if (isSyn)
                        ListenSyn();
                    //else
                    //    ListenAsyn();
                }
            });
        }

        /// <summary>
        /// 同步监听
        /// </summary>
        private void ListenSyn()
        {
            try
            {
                var context = listener.GetContext();
                //同步模式下，如果【DoWork】非常耗时，就不能继续响应后续的其他请求
                DoWork(context);
            }
            catch (HttpListenerException ex)
            {
                Logger.Warn(ex.Message);
            }
        }

        ///// <summary>
        ///// 异步监听
        ///// </summary>
        //private void ListenAsyn()
        //{
        //    var result = listener.BeginGetContext(r =>
        //    {
        //        HttpListener listener2 = (HttpListener)r.AsyncState;
        //        try
        //        {
        //            HttpListenerContext context2 = listener2.EndGetContext(r);
        //            //异步模式下，在EndGetContext执行之前，会阻塞当前线程；但是只要EndGetContext执行之后，【result.AsyncWaitHandle.WaitOne()】语句便会接到信号，后面的【DoWork】无论耗时与否，不影响后续其他请求的响应
        //            DoWork(context2);
        //        }
        //        catch (HttpListenerException ex)
        //        {
        //            Logger.Warn(ex.Message);
        //        }
        //    }, listener);
        //    result.AsyncWaitHandle.WaitOne();
        //}

        private void DoWork(MyListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request == null || request.Url == null)
                return;

            #region 输出提示
            Logger.Info(string.Format("Connect coming...{0}", request.RemoteEndPoint));
            Logger.Info(string.Format("Request:{0}", request.Url.ToString()));
            Logger.Info("Request heads:");
            foreach (string i in request.Headers)
            {
                Logger.Info(string.Format("    {0}:{1}", i, request.Headers[i]));
            }
            #endregion

            //解析URL并路由到适当的处理程序

            //用职责链模式依次解析URL
            if (urlHandler != null)
            {
                if (urlHandler.CanHandle(request))
                {
                    urlHandler.SendResponse(request, response);
                    Logger.Info(string.Format("【ok】Resolved:{0}", request.Url.ToString()));
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ResponseString("Not Found!");
                    Logger.Warn(request.Url.ToString() + " => Can't be resolved by any router!");
                }
            }

            #region 输出提示
            Logger.Info(Environment.NewLine);
            Logger.Info(Environment.NewLine);
            #endregion
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void StopServer()
        {
            isServerRunning = false;

            try
            {
                if (listener != null)
                {
                    listener.StopListen();
                }
            }
            finally
            {
                listener.StopListen();
                Logger.Info("Server shut down!");
            }
        }

        public void DelConfig()
        {
            Config.Del();//从配置文件中删除
        }
    }
}
