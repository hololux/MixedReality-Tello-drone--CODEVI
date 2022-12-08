using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Net;

public class UDPClient : MonoBehaviour

{
    // Start is called before the first frame update
    private UdpClient udpClient; 
    //public UdpClient udpClientB = new UdpClient();

    public string message;

    public void ConnectToTello()
    {
        udpClient =  new UdpClient();
        udpClient.Connect("192.168.10.1", 8889);
        IntitiateSDK();
        Recieve();
        

    }

    private void IntitiateSDK()
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

    void SendtoDrone(string message)
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

    private void OnDestroy()
    {
        DisconnectTello();
    }

    public void DisconnectTello()
    {
        udpClient.Close();
        ///udpClientB.Close();

    }
    void Recieve()
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

        

    }
        

    public void CheckBattery() {
        SendtoDrone("battery?");
        //Recieve();
    }

    public void TakeOff()
    {
        SendtoDrone("takeoff");
        Recieve();
        SendtoDrone("cw 90");
        Recieve();
        SendtoDrone("up 20");
        Recieve();
        SendtoDrone("land");
        Recieve();
        //Recieve();

    }
    



}




