using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHandler : MonoBehaviour
{
    private UDPClient_Tello telloClient;
    private UDPServer_Tello telloStateServer;
    //private UDPClient_Tello telloStateClient;

    private void Start()
    {
        telloClient = new UDPClient_Tello();
        telloStateServer = new UDPServer_Tello();

        
    }

    public void ConnectToTello()
    {
        telloClient.ConnectToTello("192.168.10.1", 8889);
        telloClient.IntitiateSDK();
        Debug.Log(Time.realtimeSinceStartup);
        Sleep(3);
        Debug.Log(Time.realtimeSinceStartup);
        telloStateServer.StartServer(8890);
    }

  
    public void CheckBattery()
    {

        telloClient.SendtoDrone("battery?");
        //Recieve();
    }

    public void TakeOff()
    {
        telloClient.SendtoDrone("takeoff");

    }

    public void MoveForward()
    {
        telloClient.SendtoDrone("forward 20");
    }

    public void land()
    {
        telloClient.SendtoDrone("land");
    }

    public void spin()
    {
        Sleep(5);
        telloClient.SendtoDrone("cw 90");
        Sleep(5);
    }
    public void Square()
    {
        telloClient.SendtoDrone("back 50");
        Sleep(8);
        telloClient.SendtoDrone("left 50");
        Sleep(8);
        telloClient.SendtoDrone("forward 50");
        Sleep(8);
        telloClient.SendtoDrone("right 50");
    }


    public void Bounce()
    {
        int verticalSpeed = 20;
        int distance = 60;
        int times = 2;
        int bounceDelay = distance / verticalSpeed;
        telloClient.SendtoDrone("up 90");
        for (int i = 0; i < times; i++)
        {
            telloClient.SendtoDrone("down" + " " + distance.ToString());
            Sleep(5);
            telloClient.SendtoDrone("up" + " " + distance.ToString());
            Sleep(5);
        }

    }

    public void Flip()
    {
        telloClient.SendtoDrone("flip r");
    }

    IEnumerator Sleep(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
