using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MyHttpServer
{
    public static class SocketExt
    {
        /// <summary>
        /// 从Socket接收数据
        /// </summary>
        /// <remarks>
        /// 由于Socket自带的Receive方法需要一个byte[]类型的参数，而该数组的长度又不好确定，
        /// 所以定义本方法，以指定步长分布从Socket接收数据，并一次性返回接收结果
        /// </remarks>
        /// <param name="socket"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static byte[] ReceiveConveniently(this Socket socket, int step = 10240)
        {
            List<byte> list = new List<byte>();
            byte[] buf = new byte[step];
            do
            {
                if (socket.Available <= 0)
                    break;
                int r = socket.Receive(buf);
                if (r == 0)
                    break;
                if (r < step)
                    buf = buf.Take(r).ToArray();
                list.AddRange(buf);
                Array.Clear(buf, 0, buf.Length);//清空buf缓冲区
            } while (true);
            return list.ToArray();
        }
    }
}
