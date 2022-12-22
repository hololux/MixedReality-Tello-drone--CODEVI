using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class UDPTelloVideo : MonoBehaviour
{
    private const float INTERVAL = 0.2f;
    public event Action<string> OnReceive;

    private string localIp = "";
    private int localPort = 11111;
    private UdpClient udpClient;
    private IPEndPoint telloAddress;

    private void Start()
    {
        
        udpClient = new UdpClient(localPort);
        string telloIp = "192.168.10.1";
        int telloPort = 8889;
        telloAddress = new IPEndPoint(IPAddress.Parse(telloIp), telloPort);
        udpClient.Send(System.Text.Encoding.UTF8.GetBytes("command"), "command".Length, telloAddress);
        //Debug.Log("sleep");
        //StartCoroutine(Sleep(2));
        //udpClient.Send(System.Text.Encoding.UTF8.GetBytes("streamon"), "streamon".Length, telloAddress);
        StartListening();

    }


    private void StartListening()
    {
        var task = Task.Run(() =>
        {
            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] response = udpClient.Receive(ref remote);
                string responseString = System.Text.Encoding.UTF8.GetString(response);
                OnReceive?.Invoke(responseString);
                Debug.Log("This is the message you received " +
                                responseString.ToString());
                Debug.Log("This message was sent from " +
                                            remote.Address.ToString() +
                                            " on their port number " +
                                            remote.Port.ToString());
                Debug.Log(responseString);


                if (responseString == "ok")
                {
                    return;
                }

                string outString = responseString.Replace(";", ";\n");
                outString = "Tello Stream data:\n" + outString;
                Debug.Log(outString);
            }
        });
    }


    IEnumerator Sleep(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}

/**private void Update()
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
}**/