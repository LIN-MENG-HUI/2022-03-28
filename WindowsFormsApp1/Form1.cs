using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UdpClient U;
        Thread Th;

        private void Listen() //監聽副程序
        {
            int Port = int.Parse(textBox_listenPort.Text);//監聽用的通訊埠
            U = new UdpClient(Port);//監聽UDP監聽器實體
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);//建立本機端點資訊

            while (true)
            {
                byte[] B = U.Receive(ref EP);//接收到的訊息放到B陣列內
                textBox_receiveMsg.Text = Encoding.Default.GetString(B); //將陣列翻譯為字串
            }
        }
        private void button_startListen_Click_1(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Th = new Thread(Listen);
            Th.Start();
            button_startListen.Enabled = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try 
            { 
                Th.Abort();
                U.Close();
            }
            catch
            {
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox_targetIP.Text;
            int Port = int.Parse(textBox_targetPort.Text);
            byte[] B = Encoding.Default.GetBytes(textBox_sendMsg.Text);
            UdpClient S = new UdpClient();
            S.Send(B, B.Length, IP, Port);
            S.Close();
        }

        private string MyIP()
        {
            string hostname = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostEntry(hostname).AddressList;

            foreach (IPAddress it in ip)
            {
                if (it.AddressFamily == AddressFamily.InterNetwork)
                {
                    return it.ToString();
                }
            }
            return "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "我的IP:" + MyIP();
        }
    }
}
