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
            catch(Exception ex)
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
                return;
            }
            int ckSum_buffer_length=(int)Math.Ceiling(((double)index)/2);
            UInt16[] ckSum_buffer = new UInt16[ckSum_buffer_length];
            int icmp_header_buffer_index = 0;
            for (int i = 0; i < ckSum_buffer_length; i++)
            {
                ckSum_buffer[i] = BitConverter.ToUInt16(buffer, icmp_header_buffer_index);
                icmp_header_buffer_index += 2;
            }
            icmp.My_CheckSum = Icmp.SumOfCheck(ckSum_buffer);
            //icmp数据包的内容
            Byte[] sendData = new Byte[packSize];
            index = icmp.CountByte(sendData);
            if (index != packSize)
            {
                listInfo.Items.Add("报文出现错误！");
                return;
            }
            int pingNUm = 4;
            for (int i = 0; i < 4; i++)
            {
                int Nbytes = 0;
                int startTime = Environment.TickCount;
                try
                {
                    Nbytes = socket.SendTo(sendData, packSize, SocketFlags.None, hostPoint);
                }
                catch (Exception ex)
                {
                    listInfo.Items.Add("发送报文失败！");
                    return;
                }
                Byte[] receiveData = new Byte[256];
                Nbytes = 0;
                int timecountsume = 0;
                while (true)
                {
                    try
                    {
                        Nbytes = socket.ReceiveFrom(receiveData, 256, SocketFlags.None, ref hostPoint);
                    }
                    catch (Exception ex)
                    {
                        listInfo.Items.Add("超时无响应！");
                        return;
                    }
                    if (Nbytes > 0)
                    {
                        timecountsume = Environment.TickCount - startTime;
                        if (timecountsume < 1)
                        {
                            listInfo.Items.Add(string.Format("来自 {0} 的回复: 字节={1} 时间<1ms", host.AddressList[0].ToString(), Nbytes));
                        }
                        else
                        {
                            listInfo.Items.Add(string.Format("来自 {0} 的回复: 字节={1} 时间={2}ms", host.AddressList[0].ToString(), Nbytes,timecountsume));
                        }
                        break;
                    }
                }
            }
            socket.Close();
        }
    }
}

