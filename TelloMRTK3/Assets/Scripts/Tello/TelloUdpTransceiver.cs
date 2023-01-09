using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


namespace Hololux.Tello
{
    public class TelloUdpTransceiver
    {
        private readonly IPEndPoint endPoint;
        private readonly UdpClient client = new UdpClient();

        public TelloUdpTransceiver(IPAddress ipAddress, int port)
        {
            endPoint = new IPEndPoint(ipAddress, port);
        }

        public void Send(string command)
        {
            byte[] bytesData =  Encoding.UTF8.GetBytes(command);
            client.Send(bytesData, bytesData.Length, endPoint);  
        }
    }
}
