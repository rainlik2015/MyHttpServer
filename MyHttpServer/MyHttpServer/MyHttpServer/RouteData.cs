using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpServer
{
    /// <summary>
    /// 路由结构数据
    /// </summary>
    public class RouteData
    {
        public string Name { set; get; }
        public string Url { set; get; }
        public StringDictionary Defaults { set; get; }

        private StringDictionary _KeyValueDic;
        public StringDictionary KeyValueDic
        {
            set
            {
                _KeyValueDic = value;
            }
            get
            {
                if (_KeyValueDic == null)
                    _KeyValueDic = Defaults;
                return _KeyValueDic;
            }
        }
    }
}
