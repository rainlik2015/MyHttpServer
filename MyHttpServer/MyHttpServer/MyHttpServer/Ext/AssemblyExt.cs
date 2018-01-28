using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MyHttpServer
{
    public static class AssemblyExt
    {
        public static string GetNameSpace(this Assembly asm)
        {
            return asm == null ? null : asm.GetTypes().FirstOrDefault().Namespace.Split('.').FirstOrDefault();
        }
    }
}
