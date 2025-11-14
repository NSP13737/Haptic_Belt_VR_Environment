using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor.PackageManager;

public class UDP_Manager : MonoBehaviour
{
    float[] float_data = new float[8];

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public void setFloats(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            float_data[i] = data[i];
        }
    }

    byte[] ProcessUDP(float[] dists)
    {
        int totalSize = dists.Length * sizeof(float);
        byte[] sendBuffer = new byte[totalSize];

        Buffer.BlockCopy(dists, 0, sendBuffer, 0, sendBuffer.Length);
        
        return sendBuffer;
    }

    // Start is called before the first frame update
    void Start()
    {
        string esp32IP = "192.168.4.1";
        int port = 4210;

        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(esp32IP), port);
        //udpClient.Connect(remoteEndPoint);

        Debug.Log("UDP Sender Ready");
    }

    // Update is called once per frame
    void Update()
    {
        byte[] rawDistsToSend = ProcessUDP(float_data);
        udpClient.Send(rawDistsToSend, rawDistsToSend.Length, remoteEndPoint);
        Debug.Log(float_data[0] + " : " + float_data[1] + " : " + float_data[2] + " : " + float_data[3] + " : " + float_data[4] + " : " + float_data[5] + " : " + float_data[6] + " : " + float_data[7]);
    }
}
