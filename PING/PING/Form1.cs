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
            EndPoint hostPoint = (new IPEndPoint(host.AddressList[0], 0)) as EndPoint;
            IPHostEntry ClientInfo;
            ClientInfo = Dns.GetHostEntry(hostClient);
            EndPoint clientPoint = (new IPEndPoint(ClientInfo.AddressList[0], 0)) as EndPoint;
            int dataSize = 4;
            int packSize = dataSize + 8;
            //请求响应，类型8
            const int  icmp_echo=8;
            Icmp icmp = new Icmp(icmp_echo, 0, 0, 45, 0, dataSize);
            Byte[] buffer = new Byte[packSize];
            int index = icmp.CountByte(buffer);
            if (index != packSize)
            {
                listInfo.Items.Add("报文出现错误！");
            }
        }
    }
}

