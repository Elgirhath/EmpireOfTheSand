using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Map;
using Assets.Units;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Ai
{
    public class AiManager : MonoBehaviour
    {
        public PlayerColor playerColor;

        private PairSocket socket;
        private NetMQPoller poller;
        private bool waitingForResponse;
        private MacroStateProvider macroStateProvider;

        private ActionController actionController;

        private Queue<Action> actionsToExecute = new Queue<Action>();

        void Start()
        {
            macroStateProvider = new MacroStateProvider(playerColor);
            actionController = GetComponent<ActionController>();

            StartConnection();
        }

        private void StartConnection()
        {
            AsyncIO.ForceDotNet.Force();
            socket = new PairSocket();
            socket.Connect("tcp://localhost:5556");
            socket.SendFrameEmpty();

            poller = new NetMQPoller { socket };

            socket.ReceiveReady += OnReceive;
            poller.RunAsync();
        }

        private void Update()
        {
            ProcessEnqueuedActions();

            if (!waitingForResponse)
            {
                GetAction();
            }
        }

        private void ProcessEnqueuedActions()
        {
            while (actionsToExecute.Count > 0)
            {
                var action = actionsToExecute.Dequeue();
                actionController.ExecuteAction(action);
            }
        }

        private void GetAction()
        {
            var state = macroStateProvider.GetState();
            var stateString = JsonConvert.SerializeObject(state);
            socket.SendFrame(stateString);
            waitingForResponse = true;
        }


        private void OnReceive(object sender, NetMQSocketEventArgs args)
        {
            var msg = args.Socket.ReceiveFrameString();

            var valid = Enum.TryParse<Action>(msg, out var action);
            if (!valid)
            {
                Debug.LogError($"Action not valid: {msg}");
                return;
            }

            actionsToExecute.Enqueue(action);

            waitingForResponse = false;
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
}
