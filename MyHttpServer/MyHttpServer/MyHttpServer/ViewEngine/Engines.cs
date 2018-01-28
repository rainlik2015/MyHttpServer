using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer.ViewEngine
{
    public static class Engines
    {
        private static TemplateEngine _engine = new MyRazorEngine();
        public static TemplateEngine Engine
        {
            get
            {
                return _engine;
            }
            set
            {
                _engine = value;
            }
        }
    }
}
