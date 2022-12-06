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
    void Start()
    {

        /**
         * 
# Function to send messages to Tello
def send(message):
try:
sock.sendto(message.encode(), tello_address)
print("Sending message: " + message)
except Exception as e:
print("Error sending: " + str(e))
         * **/
        // This constructor arbitrarily assigns the local port number.
        // Tello Address= ('192.168.10.1', 8889)
        //UdpClient udpClient = new UdpClient(8889);
        try
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("192.168.10.1", 8889);

            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes("command");

            udpClient.Send(sendBytes, sendBytes.Length);

            // Sends a message to a different host using optional hostname and port parameters.
             UdpClient udpClientB = new UdpClient();
            udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

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

            udpClient.Close();
            udpClientB.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
