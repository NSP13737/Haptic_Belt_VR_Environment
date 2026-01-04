using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDP_Manager : MonoBehaviour
{
    //TODO: this should probably be static so that our params save between scenes, but I'm can't test this yet, so I'm leaving this comment here
    float[] float_data = new float[16];

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public void setDistances(float[] data)
    {
        for (int i = 0; i < 8; i++)
        {
            float_data[i] = data[i];
        }
    }

    public void setStudyParams(float[] data)
    {
        for (int i = 0; i < 8; i++)
        {
            //Offset float data by 8 because it already contains distance values in first 8 entries
            float_data[i+8] = data[i];
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
        byte[] rawDataToSend = ProcessUDP(float_data);
        udpClient.Send(rawDataToSend, rawDataToSend.Length, remoteEndPoint);
        /*Debug.Log(float_data[0] + " : " + float_data[1] + " : " + float_data[2] + " : " + float_data[3] + " : " + float_data[4] + " : " + float_data[5] + " : " + float_data[6] + " : " + float_data[7] + "\n" +
            "conditionSelection: " + float_data[8] + "\n" +
            "minActivationDist: " + float_data[9] + "\n" +
            "maxActivationDist: " + float_data[10] + "\n" +
            "minFreqHz: " + float_data[11] + "\n" +
            "maxFreqHz: " + float_data[12] + "\n" +
            "fixedDutyCycle: " + float_data[13] + "\n" +
            "fixedFreqHz: " + float_data[14] + "\n" +
            "just_detectable_intensity" + float_data[15]);*/
    }
}
