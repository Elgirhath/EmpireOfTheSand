import zmq
import time
import json
import random
import numpy as np
from agent import DQNAgent
import zmq.asyncio
import asyncio
from tornado import gen, ioloop
# from zmq.eventloop.future import Context
from zmq.eventloop.zmqstream import ZMQStream

context = zmq.Context()
socket = None

# async def connect():
# global socket
socket = context.socket(zmq.PAIR)
socket.bind(f"tcp://*:5556")
socket.recv() # waits for client empty frame

# asyncio.run(connect())

# Exploration settings
epsilon = 1  # not a constant, going to be decayed
EPSILON_DECAY = 0.99975
MIN_EPSILON = 0.001
        
agent = None

actions = [b"BuildSandStorage", b"BuildWaterStorage", b"BuildCastle", b"Wait"]

def get_action(state):
    if np.random.random() > epsilon:
        # Get action from Q table
        return actions[np.argmax(agent.get_qs(state.values()))]
    else:
        # Get random action
        return actions[np.random.randint(0, len(actions))]
        
previous_state_action = None

def process_request(msg_multipart):
    print(f"Received message: {msg_multipart[0]}")
    rq = json.loads(msg_multipart[0].decode('utf-8'))
    
    reward = rq['Reward']
    game_ended = rq['GameEnded']
    state = rq['State']

    action = get_action(state)

    global agent
    if agent is None:
        agent = DQNAgent(len(state.keys()), len(actions))
    
    global previous_state_action
    if previous_state_action is not None:
        prev_state = previous_state_action[0]
        prev_action = previous_state_action[1]
        agent.update_replay_memory((prev_state, actions.index(prev_action), reward, state, game_ended))
        agent.train()

    previous_state_action = (state, action)

    socket.send(action)
    print(f"Action sent: {action}")

    global epsilon
    epsilon = max(MIN_EPSILON, epsilon * EPSILON_DECAY)

stream = ZMQStream(socket)
stream.on_recv(process_request)
ioloop.IOLoop.instance().start()