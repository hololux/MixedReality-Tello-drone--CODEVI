using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;

public class UDPServer_Tello : MonoBehaviour
{

    private UdpClient udpServer;
    private IPEndPoint _listenOn;
    public event Action<string> OnReceive;

   

    public void StartServer(int port)
    {
        _listenOn = (new IPEndPoint(IPAddress.Parse("0.0.0.0"), port));
        udpServer = new UdpClient(port);
        StartListening();
    }

    private void StartListening()
    {
        var task = Task.Run(() =>
        {
            while (true)
            {
                // 



                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _listenOn.Port);
                byte[] receiveBytes = udpServer.Receive(ref groupEP);
                string receivedString = Encoding.ASCII.GetString(receiveBytes);
                OnReceive?.Invoke(receivedString);
                Debug.Log("This is the message you received from Server " +
                                receivedString.ToString());
                Debug.Log("This message was sent from  Server " +
                                            groupEP.Address.ToString() +
                                            " on their port number " +
                                            groupEP.Port.ToString());
            }
        });
    }


}
