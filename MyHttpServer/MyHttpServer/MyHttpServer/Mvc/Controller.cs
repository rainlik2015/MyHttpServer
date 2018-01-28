using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer.ActionResults;

namespace MyHttpServer.Mvc
{
    public abstract class Controller
    {
        protected ViewResult View(string viewName = null)
        {
            return new ViewResult(viewName);
        }
        protected ViewResult View(ViewPage viewPage = null)
        {
            return new ViewResult(viewPage);
        }
        protected ContentResult Content(string arg)
        {
            return new ContentResult(arg);
        }
    }
}
