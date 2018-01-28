using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyHttpServer.Mvc;

namespace MyHttpServer.ViewEngine
{
    public abstract class TemplateEngine
    {
        public MvcContext Ctx { set; get; }
        /// <summary>
        /// 翻译.cshtml文件为C#类
        /// </summary>
        /// <param name="csHtml">.cshtml文件</param>
        /// <returns>C#类</returns>
        protected abstract string TranslateView(string csHtml);

        /// <summary>
        /// 把C#类编译成为程序集，保存到磁盘，并加载到当前内存
        /// </summary>
        /// <param name="csFile">C#类</param>
        /// <returns>程序集</returns>
        protected abstract Assembly CompileView(string csFile);

        public Assembly DoWork(string csHtml)
        {
            var csFile = TranslateView(csHtml);
            return CompileView(csFile);
        }
    }
}
