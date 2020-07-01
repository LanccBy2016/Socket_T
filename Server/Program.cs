using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            if (args.Length > 0)
            {
                port = int.Parse(args[0]);
            }
            else
            {
                port = 1157;
            }
            ServerControl serverControl = new ServerControl();
            serverControl.Start(port);
            Console.WriteLine("127.0.0.1:{0} --服务启动成功",port);
            Console.ReadKey();
        }
    }
}
