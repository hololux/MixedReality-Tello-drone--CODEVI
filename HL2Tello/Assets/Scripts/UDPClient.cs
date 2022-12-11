using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;

public class UDPClient : MonoBehaviour

{
    // Start is called before the first frame update
    private UdpClient udpClient;
    public event Action<string> OnReceive;
    //public UdpClient udpClientB = new UdpClient();

    public string message;

    public void ConnectToTello()
    {
        udpClient =  new UdpClient();
        udpClient.Connect("192.168.10.1", 8889);
        //StartListening();
        IntitiateSDK();
        
        //Recieve();
        

    }

    private void Update()
    {
        StartListening();
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


    public void CheckBattery() {
        SendtoDrone("battery?");
        //Recieve();
    }

    public void TakeOff()
    {
        SendtoDrone("takeoff");
        //Recieve();
        //Recieve();

    }

    public void MoveForward()
    {
        SendtoDrone("forward 20");

    }

    public void land()
    {
        SendtoDrone("land");

    }

    public void spin()
    {
        Sleep(5);
        SendtoDrone("cw 90");
        Sleep(5);
    }
    public void Square()
    {
        SendtoDrone("back 50");
        Sleep(8);
        SendtoDrone("left 50");
        Sleep(8);
        SendtoDrone("forward 50");
        Sleep(8);
        SendtoDrone("right 50");
    }


    public void Bounce()
    {
        int verticalSpeed = 20;
        int distance=60;
        int times=3;
        int bounceDelay = distance / verticalSpeed;
        for (int i = 0; i < times; i++)
        {
            SendtoDrone("down" + distance.ToString());
            Sleep(5);
            SendtoDrone("up" + distance.ToString());
            Sleep(5);
        }

    }

    public void Flip()
    {
        SendtoDrone("flip r");
    }

    IEnumerator Sleep(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }




}




