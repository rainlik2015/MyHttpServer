using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using MyHttpServer.ViewEngine;
using MyHttpServer.Log;
using MyHttpServer.Mvc;
using System.IO;
using MyHttpServer.Listener;

namespace MyHttpServer.ActionResults
{
    public class ViewResult : ActionResult
    {
        private string viewName;
        private string actionName;
        private string controllerName;
        private ViewPage viewPage = null;

        public ViewResult(ViewPage view)
        {
            viewPage = view;
        }
        public ViewResult(string vName = null)
        {
            if (vName != null)
            {
                actionName = vName;
            }
        }
        public BaseLog Logger { set; get; }
        public override Stream GetHttpBody(MvcContext ctx)
        {
            if (viewPage == null)
                viewPage = FindView(actionName, ctx);
            if (viewPage == null)
                return null;
            return viewPage.GetBytes();
        }

        private void InitContextFields(MvcContext ctx)
        {
            if (actionName == null)
                actionName = ctx.ActionName;
            controllerName = ctx.ControllerName;
           Engines.Engine.Ctx = ctx;
        }

        private Type GetViewCode(Assembly assembly)
        {
            viewName = string.Format("{0}.Views.{1}.{2}", assembly.GetNameSpace(), controllerName, actionName);
            var tempViewType = assembly.GetType(viewName);
            return tempViewType;
        }

        private string GetCsHtmlFile(MvcContext ctx)
        {
            //MyWeb\Views\Home\Index.cshtml
            string viewTemplateName = string.Format(@"Views\{0}\{1}.cshtml", controllerName, actionName);
            var basePath = ctx.PhysicalPath;//此处basePath代表网站文件所在目录

            var csHtmlPath = basePath + viewTemplateName;
            return csHtmlPath;
        }

        /// <summary>
        /// 根据视图名称寻找相应的ViewPage
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private ViewPage FindView(string viewName, MvcContext ctx)
        {
            ViewPage r = null;

            #region 初始化上下文字段
            InitContextFields(ctx);
            #endregion

            #region 寻找视图模版（.cshtml文件）
            var csHtml = GetCsHtmlFile(ctx);
            //var csHtml = GetCsHtmlFile();
            #endregion

            #region 把视图模版编译成临时视图类
            var viewAssembly = Engines.Engine.DoWork(csHtml);
            #endregion

            #region 定位临时视图类
            var viewPageType = GetViewCode(viewAssembly);
            r = Activator.CreateInstance(viewPageType) as ViewPage;
            #endregion

            return r;
        }
    }
}
