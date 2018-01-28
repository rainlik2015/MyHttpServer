using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MyHttpServer.Common
{
    public class Gzip
    {
        public static byte[] GzipBytes(byte[] buf)
        {
            byte[] r = buf;
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream compressedZipStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    compressedZipStream.Write(buf, 0, buf.Length);
                    compressedZipStream.Close();
                }
                r = new byte[ms.Length];
                ms.Position = 0;//此处很重要！因为compressedZipStream在把压缩后的字节写入内存流ms后，ms的指针已经移动到了最后。此时，如果要想从内存流ms中读取数据，首先要把读指针复位到0处
                ms.Read(r, 0, r.Length);
                ms.Close();
            }
            return r;
        }
    }
}
