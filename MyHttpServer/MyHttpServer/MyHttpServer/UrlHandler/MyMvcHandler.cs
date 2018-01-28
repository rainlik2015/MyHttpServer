using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using MyHttpServer.ModelBinder;
using MyHttpServer.ActionResults;
using MyHttpServer.Common;
using MyHttpServer.Mvc;
using MyHttpServer.Cache;
using MyHttpServer.Listener;

namespace MyHttpServer.UrlRoute
{
    /// <summary>
    /// MVC路由
    /// </summary>
    public class MyMvcHandler:BaseUrlHandler
    {
        private Type controllerType = null;
        private MethodInfo actionMethod = null;
        private List<MethodInfo> actionMethods = new List<MethodInfo>();
        private RouteData matchedUrlRule;
        private Assembly userAssembly;
        List<object> realParam = new List<object>();//Action的参数集合

        public MyMvcHandler(string siteName, string path, ICache c = null)
            : base(siteName, path, c)
        {
            InitUrlRules();
            
        }
        /// <summary>
        /// 初始化路由规则列表
        /// </summary>
        private void InitUrlRules()
        {
            //默认值、可选值的实现？
            StringDictionary defaults = new StringDictionary();
            defaults.Add("Controller", "Home");
            defaults.Add("Action", "GetUser");
            defaults.Add("Id", "0");

            UrlRouteConfig.UrlRoutes.Add(new RouteData { Name = "myDefault", Url = @"my{Controller}/my{Action}/my{Id}", Defaults = defaults });
            UrlRouteConfig.UrlRoutes.Add(new RouteData { Name = "Default", Url = @"{Controller}/{Action}/{Id}", Defaults = defaults });
        }

        /// <summary>
        /// 遍历路由规则列表，从其中寻找和浏览器请求的Url相匹配的路由规则
        /// </summary>
        /// <param name="path">浏览器请求的Url</param>
        /// <returns>匹配到的路由规则</returns>
        private RouteData MatchRoute(string path)
        {
            foreach (var r in UrlRouteConfig.UrlRoutes)
            {
                var dic = ParseUrl(path, r.Url);
                if (dic == null)
                {
                    continue;
                }
                r.KeyValueDic = dic;
                return r;
            }
            return null;
        }

        /// <summary>
        /// 解析浏览器请求的url，从中提取Controller、Action以及Id
        /// </summary>
        /// <param name="url">浏览器请求的url</param>
        /// <param name="urlRule">路由规则</param>
        /// <returns></returns>
        private StringDictionary ParseUrl(string url, string urlRule)
        {
            var x = UrlRouteConfig.UrlRoutes.Find(r =>
            {
                return true;
            });
            StringDictionary dicRouteData = new StringDictionary();

            string tempInput = urlRule;
            string keyPattern = @"\{\w+\}";
            Regex keyRegex = new Regex(keyPattern);
            var keyMatches = keyRegex.Matches(tempInput);
            List<string> keys = new List<string>();
            for (int i = 0; i < keyMatches.Count; i++)
            {
                keys.Add(keyMatches[i].ToString().TrimStart('{').TrimEnd('}'));
                tempInput = tempInput.Replace(keyMatches[i].ToString(), @"(?<{" + i + @"}>\w+)");
            }
            tempInput = string.Format("^{0}$", tempInput);

            string valuePattern = string.Format(tempInput, keys[0], keys[1], keys[2]);
            Regex valueRegex = new Regex(valuePattern);
            var valueMatch = valueRegex.Match(url);
            if (valueMatch.Success)
            {
                foreach (var key in keys)
                {
                    dicRouteData.Add(key, valueMatch.Groups[key].Value);
                }
            }
            else
            {
                return null;
            }
            return dicRouteData;
        }
        private ActionResult actionResult = null;
        public override Stream GetHttpBodyWithoutCache(MyHttpRequest req, MyHttpResponse resp)
        {
            if (actionResult == null)
                return null;

            resp.StatusCode = (int)HttpStatusCode.OK;
            resp.StatusDescription = "MyMvcOK";

            MvcContext ctx = new MvcContext
            {
                UserAssembly = userAssembly,
                ControllerName = matchedUrlRule.KeyValueDic["Controller"],
                ActionName = matchedUrlRule.KeyValueDic["Action"],
                Request = req,
                Response = resp,
                PhysicalPath = this.PhysicalPath
            };
            return actionResult.GetHttpBody(ctx);
        }

        private void InitActionResult()
        {
            var controllerInstance = Activator.CreateInstance(controllerType);
            actionResult = actionMethod.Invoke(controllerInstance, realParam.Count == 0 ? null : realParam.ToArray()) as ActionResult;
        }

        /// <summary>
        /// 检查路由规则
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool CheckUrlRules(Uri url)
        {
            string path = url.AbsolutePath.TrimStart('/');
            /*
             * {controller}/{action}/{id}==>str==>正则表达式
             * Home/GetUser/1==>path
             */
            matchedUrlRule = MatchRoute(path);
            return matchedUrlRule != null;
        }
        private object lockerObj = new object();
        public override bool CanHandle(MyHttpRequest req)
        {
            var url = req.Url;
            if(!CheckUrlRules(url))
                return false;

            #region 已知类名，利用反射获取对应的类
            //MyHttpServer类库被引用到用户项目中以后，需要在用户项目中查找URL指定的控制器类
            var controllerString = matchedUrlRule.KeyValueDic["Controller"] + "Controller";
            var actionString = matchedUrlRule.KeyValueDic["Action"];
            
            //用户网站项目编译后的dll路径
            string userDll = string.Format("{0}{1}.dll", PhysicalPath, WebSiteName);
            userAssembly = Assembly.LoadFrom(userDll);

            if (userAssembly == null)
                return false;
            controllerType = userAssembly.GetTypes().Where(t => t.Name == controllerString).FirstOrDefault(); 
            #endregion

            #region 已知方法名，获取对应的方法
            var actionMethods = controllerType.GetMethods().Where(m => m.Name == actionString).ToList();
            
            List<BaseModelBinder> modelBinders = new List<BaseModelBinder>();//Action参数模型绑定器的集合
            modelBinders.Add(new UrlModelBinder(matchedUrlRule, req));
            modelBinders.Add(new BodyModelBinder(req));

            foreach (var act in actionMethods)//遍历所有同名方法
            {
                lock (lockerObj)
                {
                    realParam.Clear();
                    foreach (var binder in modelBinders)
                    {
                        binder.Method = act;
                        var r = binder.Parse();
                        if (r != null)
                        {
                            realParam = r;
                            break;
                        }
                    }
                    if (realParam == null)
                        continue;
                    //if (realParam.Count == 0)
                    //    continue;
                    if (act.GetParameters().Length != realParam.Count)
                        continue;
                    actionMethod = act;
                    break;
                }
            } 
            #endregion

            InitActionResult();

            return controllerType != null && actionMethod != null;
        }
    }
}
