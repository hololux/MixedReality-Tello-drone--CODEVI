using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;

// Custom TelloVideo class to handle video streaming from the drone
public class TelloVideo : MonoBehaviour
{
    // Constants for the video stream
    private const int VIDEO_PORT = 11111;
    private const int BUFFER_SIZE = 1024;
    private const int FPS = 30;
    private UDPClient_Tello startStream;

    // Constants for the drone's SDK
    private const string SDK_HOST = "192.168.10.1";
    private const int SDK_PORT = 8889;

    // UDP client for receiving the video stream
    private UdpClient udpClient;

    // Thread for handling the video stream
    private Thread thread;

    // Buffer for storing the video data
    private byte[] buffer;

    // Flag to control the video streaming
    private bool running;

    // RawImage component to display the video stream
    private RawImage rawImage;

    // UDP client for connecting to the drone's SDK
    private UdpClient sdkClient;

    void Start()
    {
        // Connect to the drone's SDK
        ConnectToSDK();
       

        

        // Start the video stream
        StartStreaming();
    }

    // Connects to the drone's SDK
    private void ConnectToSDK()
    {
        // Create a new UDP client for connecting to the drone's SDK
        sdkClient = new UdpClient();
        sdkClient.Connect(SDK_HOST, SDK_PORT);

        // Send the "command" command to enter command mode
        byte[] command = System.Text.Encoding.ASCII.GetBytes("command\n");
        sdkClient.Send(command, command.Length);
    }

    // Starts the video streaming
    private void StartStreaming()
    {
        // Create a new UDP client for receiving the video stream
        udpClient = new UdpClient(VIDEO_PORT);

        // Create a new buffer for storing the video data
        buffer = new byte[BUFFER_SIZE];

        // Set the running flag to true
        running = true;

        // Create a new thread for handling the video stream
        thread = new Thread(new ThreadStart(ProcessStream));
        thread.Start();
    }

    // Processes the video stream
    private void ProcessStream()
    {
        // Variables for storing the video data
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, VIDEO_PORT);
        int totalBytes;

        // Receive the video data in a loop
        while (running)
        {
            // Receive the video data
            buffer = udpClient.Receive(ref endPoint);
            totalBytes = buffer.Length;

            // Update the raw image with the received data
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(buffer);
            rawImage.texture = texture;
        }
    }

}
