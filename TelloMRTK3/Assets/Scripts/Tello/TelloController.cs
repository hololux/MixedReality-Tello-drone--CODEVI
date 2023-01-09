using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Hololux.Tello
{
    public class TelloController : MonoBehaviour
    {
        public TelloVideoRenderer videoRenderer;
        private TelloUdpTransceiver telloMessenger;
        
        public void ConnecctToTello()
        {
            telloMessenger = new TelloUdpTransceiver(IPAddress.Parse("192.168.10.1"), 8889);

            string command = "command";
            telloMessenger.Send(command);
        }

        public void StartVideoStreaming()
        {
            videoRenderer.StartVideo();
            
            string videoCommand = "streamon";
            telloMessenger.Send(videoCommand);
            
            Debug.Log("Start Video Streaming");
        }
    }  
}

