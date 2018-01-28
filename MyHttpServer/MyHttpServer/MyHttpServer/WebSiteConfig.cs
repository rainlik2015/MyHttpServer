using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer
{
    public class WebSiteConfig
    {
        public string SiteName { set; get; }
        public string Port { set; get; }
        public string PhysicalPath { set; get; }

        private static string cfgPath;
        static WebSiteConfig()
        {
            var basePath = Directory.GetCurrentDirectory();
            //var basePath = AppDomain.CurrentDomain.BaseDirectory;
            cfgPath = string.Format(@"{0}\WebSiteConfig.txt", basePath); 
        }
        public bool Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(cfgPath, true, Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0}@{1}@{2}", SiteName, Port, PhysicalPath));
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Del()
        {
            try
            {
                var list = GetAllSites();
                if (list == null || list.Count == 0)
                    return false;

                using (StreamWriter sw = new StreamWriter(cfgPath,false, Encoding.UTF8))
                {
                    foreach (var s in list)
                    {
                        if (s.Port == this.Port)
                            continue;
                        sw.WriteLine(string.Format("{0}@{1}@{2}", s.SiteName, s.Port, s.PhysicalPath));
                    }
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static List<WebSiteConfig> GetAllSites()
        {
            List<WebSiteConfig> list = new List<WebSiteConfig>();
            using (StreamReader sr = new StreamReader(cfgPath, Encoding.UTF8))
            {
                do
                {
                    string r = sr.ReadLine();
                    if (r == null || r == "")
                        break;
                    var strs = r.Split('@');
                    WebSiteConfig cfg = new WebSiteConfig();
                    cfg.SiteName = strs[0];
                    cfg.Port = strs[1];
                    cfg.PhysicalPath = strs[2];
                    list.Add(cfg);
                } while (true);
            }
            return list;
        }
    }
}
