using System;
using System.Collections;
using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Units;
using UnityEngine;

namespace Ai
{
    public class AiManager : MonoBehaviour
    {
        public PlayerColor playerColor;

        private PairSocket socket;
        private NetMQPoller poller;
        private bool waitingForResponse;
        private bool waiting;
        private MacroStateProvider macroStateProvider;

        private ActionController actionController;

        private float interval = 5f;

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

            if (!waitingForResponse && !waiting)
            {
                StartCoroutine(Send());
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

        private IEnumerator Send()
        {
            waiting = true;
            yield return new WaitForSeconds(interval);
            SendRequest();
            waiting = false;
        }

        private void SendRequest()
        {
            var state = macroStateProvider.GetState();
            var stateString = JsonConvert.SerializeObject(state);
            socket.SendFrame(stateString);
            Debug.Log($"Sending frame: {stateString}");
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
