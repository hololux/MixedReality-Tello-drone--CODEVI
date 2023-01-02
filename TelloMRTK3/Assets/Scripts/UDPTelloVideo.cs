using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UDPTelloVideo : MonoBehaviour
{
    private const float INTERVAL = 0.2f;

    private string localIp = "";
    private int localPort = 11111;
    private UdpClient udpClient;
    private IPEndPoint telloAddress;

    // Add a RawImage component to display the video
    public RawImage videoImage;

    private void Start()
    {
        udpClient = new UdpClient(localPort);

        string telloIp = "192.168.10.1";
        int telloPort = 8889;
        telloAddress = new IPEndPoint(IPAddress.Parse(telloIp), telloPort);
        udpClient.Send(System.Text.Encoding.UTF8.GetBytes("command"), "command".Length, telloAddress);
        StartCoroutine(Delay(2.0f));
        udpClient.Send(System.Text.Encoding.UTF8.GetBytes("streamon"), "streamon".Length, telloAddress);
        
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = udpClient.Receive(ref remote);

        // Create a new Texture2D object and set its size based on the size of the video frame
        Texture2D videoTexture = new Texture2D(176, 144);
        videoTexture.LoadImage(response);

        // Set the texture of the RawImage component to the video frame
        videoImage.texture = videoTexture;
        Debug.Log(videoImage);


        // Start the co-routine to receive and process the video frames
        // StartCoroutine(ReceiveVideoFrames());
    }

    private IEnumerator ReceiveVideoFrames()
    {
        while (true)
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            byte[] response = udpClient.Receive(ref remote);

            // Create a new Texture2D object and set its size based on the size of the video frame
            Texture2D videoTexture = new Texture2D(176, 144);
            videoTexture.LoadImage(response);

            // Set the texture of the RawImage component to the video frame
            videoImage.texture = videoTexture;

            // Destroy the videoTexture object to avoid memory leaks
            Destroy(videoTexture);

            yield return new WaitForSeconds(INTERVAL);
        }
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}