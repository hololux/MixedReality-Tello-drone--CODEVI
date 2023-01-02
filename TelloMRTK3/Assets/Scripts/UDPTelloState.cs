using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPTelloState : MonoBehaviour
{
    private const float INTERVAL = 0.2f;

    
    private string localIp = "";
    private int localPort = 8890;
    private UdpClient udpClient;
    private IPEndPoint telloAddress;

    private void Start()
    {
        udpClient = new UdpClient(localPort);

        string telloIp = "192.168.10.1";
        int telloPort = 8889;
        telloAddress = new IPEndPoint(IPAddress.Parse(telloIp), telloPort);

        udpClient.Send(System.Text.Encoding.UTF8.GetBytes("command"), "command".Length, telloAddress);
    }

    private void Update()
    {
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = udpClient.Receive(ref remote);
        string responseString = System.Text.Encoding.UTF8.GetString(response);

        if (responseString == "ok")
        {
            return;
        }

        string outString = responseString.Replace(";", ";\n");
        outString = "Tello State:\n" + outString;
        Debug.Log(outString);
        Invoke("Update", INTERVAL);
    }
}