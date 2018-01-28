using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHttpServer
{
    public static class StreamExt
    {
        /// <summary>
        /// 一次性读取流中的数据
        /// </summary>
        /// <remarks>
        /// Stream自带的Read方法不太方便，需要提供一个byte[]数组，该数组长度不好确定，
        /// 所以自己写一个方法以便于操作
        /// </remarks>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            List<Byte> list = new List<byte>();
            stream.ReadAllByStep(b =>
            {
                list.AddRange(b);
            });
            byte[] buf = list.ToArray();
            return buf;
        }

        /// <summary>
        /// 从流中循环分块读取数据，从而避免处理大数据时内存不足
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="act">读取每块数据后进行的操作</param>
        /// <param name="step">每次读取的数据块大小</param>
        public static void ReadAllByStep(this Stream stream, Action<byte[]> act, int step = 10240)
        {
            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
            byte[] buf = new byte[step];
            do
            {
                int r = stream.Read(buf, 0, buf.Length);
                if (r == 0)
                    break;
                if (r < step)
                    buf = buf.Take(r).ToArray();
                if (act != null)
                    act(buf);
                Array.Clear(buf, 0, buf.Length);//清空buf缓冲区
            } while (true);
        }
    }
}
