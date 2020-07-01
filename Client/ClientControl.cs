using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public class ClientControl
    {
        private Socket clientSocket;
        public ClientControl()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(string ip, int port)
        {
            //连接到服务端Socket
            clientSocket.Connect(ip, port);
            //启动一个后台进程，持续接收服务端消息
            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        /// <summary>
        /// 发送消息到服务端
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private void Receive()
        {
            try
            {
                byte[] msg = new byte[10240];
                int msglength = clientSocket.Receive(msg);
                string msgStr = Encoding.UTF8.GetString(msg, 0, msglength);
                Form1.form1.ShowMsg(msgStr);
                Receive();
            }
            catch
            {
                MessageBox.Show("服务器异常");
            }
        }
    }
}
