using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer
{
    public static class StringExt
    {
        public static StringDictionary GetData(this string str)
        {
            StringDictionary r = new StringDictionary();
            var segments = str.Trim().Split('&');
            if (segments == null || segments.Length == 0)
                return r;
            foreach (var segment in segments)
            {
                var items = segment.Split('=');
                if (items == null || items.Length < 2)
                    continue;
                r.Add(items[0], items[1]);
            }
            return r;
        }
        public static Stream ToStream(this string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            var buf = encoding.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            ms.Write(buf, 0, buf.Length);
            return ms;
        }
    }
}
