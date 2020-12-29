using System;
using Ai;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class AiQueueConnector : MonoBehaviour
{
    private PairSocket socket;
    private NetMQPoller poller;

    public static AiQueueConnector Instance { get; private set; }

    void Start()
    {
        Instance = this;
        StartConnection();
    }

    private void StartConnection()
    {
        AsyncIO.ForceDotNet.Force();
        socket = new PairSocket();
        socket.Connect("tcp://localhost:5556");
        socket.SendFrameEmpty();

        poller = new NetMQPoller { socket };

        RegisterAiManagers();

        poller.RunAsync();
    }

    public void AddListener(Action<string> action)
    {
        socket.ReceiveReady += (sender, e) => action(e.Socket.ReceiveFrameString());
    }

    private void RegisterAiManagers()
    {
        var aiManagers = FindObjectsOfType<AiManager>();
        foreach (var aiManager in aiManagers)
        {
            AddListener(aiManager.OnReceive);
        }
    }

    public void Send(string msg)
    {
        socket.SendFrame(msg);
        Debug.Log($"Sending frame: {msg}");
    }

    private void Cleanup()
    {
        poller.Dispose();
        socket.Dispose();
        NetMQConfig.Cleanup();
    }

    void OnApplicationQuit()
    {
        Cleanup();
    }
}
