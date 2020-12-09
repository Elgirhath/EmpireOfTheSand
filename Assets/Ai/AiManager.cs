using System;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

namespace Assets.Ai
{
    public class AiManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AsyncIO.ForceDotNet.Force();
            using (var server = new PairSocket())
            {
                server.Connect("tcp://localhost:5556");
                server.SendFrameEmpty();
                var received = server.TryReceiveFrameString(TimeSpan.FromMilliseconds(100), out var msg);
                Debug.Log(msg);
            }
            NetMQConfig.Cleanup();
        }
    }
}
