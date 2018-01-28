using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer
{
    public static class DateTimeExt
    {
        public static DateTime OmitMillisecond(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }
    }
}
