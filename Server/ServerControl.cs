using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class ServerControl
    {
        private Socket serverSocket;
        //客户端列表
        List<Socket> clientList;
        public ServerControl()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            clientList = new List<Socket>();
        }
        public void Start(int prot)
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, prot));
            //客户端最大等待数
            serverSocket.Listen(10);
            Console.WriteLine("服务器启动成功！");
            //开启一个后台线程用于监听客户端链接
            Thread threadAccept = new Thread(Accept);
            threadAccept.IsBackground = true;
            threadAccept.Start();
        }


        /// <summary>
        /// 监听客户端链接
        /// </summary>
        private void Accept()
        {
            //serverSocket.Accept()会挂起当前进程，直至有客户端请求链接。
            var client = serverSocket.Accept();
            //Endpoint转成IpEndPoint
            var point = client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine("{0}:{1}链接成功", point.Address, point.Port);
            //将客户端添加到连接列表
            clientList.Add(client);
            //与客户端链接后，使用Receive获取客户端传递的消息
            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start(client);
            //递归，继续监听客户端链接请求
            Accept();
        }

        /// <summary>
        /// 接收客户端消息
        /// </summary>
        /// <param name="obj">Socket客户端</param>
        private void Receive(object obj)
        {
            Socket client = obj as Socket;
            var point = client.RemoteEndPoint as IPEndPoint;
            try
            {
                var msg = new byte[10240];
                //Receive方法会挂起当前进程，直至客户端发起消息
                int msglength = client.Receive(msg);
                string msgStr = Encoding.UTF8.GetString(msg, 0, msglength);
                msgStr = string.Format("【{0}】【{1}:{2}】：{3}", DateTime.Now.ToString("HH:mm:ss"), point.Address, point.Port, msgStr);
                Console.WriteLine(msgStr);
                //将消息广播
                Broadcast(client, msgStr);
                //递归，持续监听此客户端消息
                Receive(client);
            }
            catch
            {
                //监听客户端途中异常，与客户端连接失效，从客户端列表中将其移除
                Console.WriteLine("【{0}:{1}】已断开链接", point.Address, point.Port);
                clientList.Remove(client);
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        private void Broadcast(Socket clientOther,string msg)
        {
            //将消息广播给除自己外的其它客户端
            foreach (var item in clientList)
            {
                if (item == clientOther)
                {
                    continue;
                }
                item.Send(Encoding.UTF8.GetBytes(msg));
            }
        }
    }
}
