using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;

public class UDPClient_Tello

{
    // Start is called before the first frame update
    private UdpClient udpClient;
    public event Action<string> OnReceive;
    //public UdpClient udpClientB = new UdpClient();


   

    public void ConnectToTello(string ip, int port)
    {

        udpClient =  new UdpClient();
        udpClient.Connect(ip, port);
        StartListening();
        //IntitiateSDK();
        
        //Recieve();
        

    }

    /**private void Update()
    {
        StartListening();
    }**/

    public void IntitiateSDK()
    {

        try
        {
            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes("command");
            udpClient.Send(sendBytes, sendBytes.Length);
            Debug.Log("Sending message: " + "command");
            //Recieve();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    public void SendtoDrone(string message)
    {
        try
        {   
            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(sendBytes, sendBytes.Length);
            //udpClientB.Send(sendBytes, sendBytes.Length, "", 9000);
            Debug.Log("Sending message: " + message);

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    

    public void DisconnectTello()
    {
        udpClient.Close();
        ///udpClientB.Close();

    }

    private void StartListening()
    {
        var task = Task.Run(() =>
        {
            while (true)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 9000);
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedString = Encoding.ASCII.GetString(receiveBytes);
                OnReceive?.Invoke(receivedString);
                Debug.Log("This is the message you received " +
                                receivedString.ToString());
                Debug.Log("This message was sent from " +
                                            remoteEndPoint.Address.ToString() +
                                            " on their port number " +
                                            remoteEndPoint.Port.ToString());
            }
        });
    }

    /**void Recieve()
    {
        try
        {

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 9000);
            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Debug.Log("This is the message you received " +
                                            returnData.ToString());
            Debug.Log("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        

    }**/


    




}




