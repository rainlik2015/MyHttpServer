using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Web.Razor.Parser;
using System.Web.Razor;

namespace MyHttpServer.ViewEngine
{
    /// <summary>
    /// 把.cshtml文件翻译成C#类
    /// </summary>
    public class MyRazorEngine : TemplateEngine
    {
        /// <summary>
        /// 【未完成】
        /// 如何把cshtml文件翻译成C#代码？语法树？涉及到编译原理的知识？
        /// </summary>
        /// <param name="csHtml"></param>
        /// <returns></returns>
        protected override string TranslateView(string csHtml)
        {
            //ViewPage viewPage = null;
            string csFile = null;

            #region 模拟
            var dir = Path.GetDirectoryName(csHtml);
            var n = Path.GetFileNameWithoutExtension(csHtml);
            //csFile = string.Format(@"{0}\{1}Cshtml.cs", dir, n);
            csFile = string.Format(@"{0}\{1}.cs", dir, n);
            #endregion

            #region 翻译cshtm文件
            //var lines = File.ReadAllLines(csHtml, Encoding.Default);
            //foreach (var line in lines)
            //{

            //}
            //File.WriteAllLines(csPath, new string[] { outputCode });
            #endregion

            return csFile;
        }

        protected override Assembly CompileView(string csFile)
        {
            return Common.Compiler.Compile(csFile);
        }
    }
}
