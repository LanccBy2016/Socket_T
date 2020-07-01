using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        ClientControl client = new ClientControl();
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (this.btn_Connect.Text == "连接")
            {
                client.Connect(this.txt_ServerIP.Text.Trim(), int.Parse(this.txt_port.Text.Trim()));
                MessageBox.Show("服务器连接成功");
            }

            
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string msg = this.txt_msg.Text;
            if (msg.Length > 0)
            {
                client.Send(msg);
            }
            this.txt_msg.Text = "";
            string msgStr = string.Format("【{0}】【你】：{1}", DateTime.Now.ToString("HH:mm:ss"), msg);
            ShowMsg(msgStr);
        }

        public void ShowMsg(string msg)
        {
            this.list_msg.Items.Add(msg);
        }
    }
}
