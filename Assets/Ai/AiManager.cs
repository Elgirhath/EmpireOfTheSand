using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Units;
using UnityEngine;

namespace Ai
{
    public class AiManager : MonoBehaviour
    {
        public PlayerColor playerColor;

        private bool waitingForResponse;
        private bool waiting;
        private MacroStateProvider macroStateProvider;

        private ActionController actionController;

        private float interval = 5f;

        private Queue<Action> actionsToExecute = new Queue<Action>();

        void Start()
        {
            macroStateProvider = GetComponent<MacroStateProvider>();
            actionController = GetComponent<ActionController>();
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
            var state = macroStateProvider.GetState(playerColor);
            var stateString = JsonConvert.SerializeObject(state);
            AiQueueConnector.Instance.Send(stateString);
        }


        public void OnReceive(string msg)
        {
            var valid = Enum.TryParse<Action>(msg, out var action);
            if (!valid)
            {
                Debug.LogError($"Action not valid: {msg}");
                return;
            }

            actionsToExecute.Enqueue(action);

            waitingForResponse = false;
        }
    }
}
