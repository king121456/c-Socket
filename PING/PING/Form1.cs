using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PING
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            listInfo.Items.Clear();
            if (txtIpAddress.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Ip地址不能为空！");
                return;
            }
            string hostClient = txtIpAddress.Text;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            socket.ReceiveTimeout = 1000;
            IPHostEntry host;
            try
            {
                host = Dns.GetHostEntry(txtIpAddress.Text);
            }
            catch
            {
                listInfo.Items.Add("无效的主机名或ip地址！");
                return;
            }
        }
    }
}
