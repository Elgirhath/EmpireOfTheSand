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
        
previous_state_actions = {}

def get_macro_state(player_data):
    state_values = list(next(player['State'].values() for player in player_data if player['Current']))
    for player in player_data:
        if player['Current']:
            continue
        state_values += list(player['State'].values())

    return state_values
        

def process_request(msg_multipart):
    print(f"Received message: {msg_multipart[0]}")
    rq = json.loads(msg_multipart[0].decode('utf-8'))
    
    game_ended = rq['GameEnded']
    player_data = rq['PlayerData']
    player = next(player for player in player_data if player['Current'])
    player_color = player['Color']
    state = get_macro_state(player_data)

    action = get_action(state)

    global agent
    if agent is None:
        agent = DQNAgent(len(state), len(actions))
    
    if player_color in previous_state_actions.keys():
        prev_state, prev_action = previous_state_actions[player_color]
        agent.update_replay_memory((prev_state, actions.index(prev_action), player['Reward'], np.array(state), game_ended))
        agent.train()

    previous_state_actions[player_color] = (state, action)

    socket.send(action)
    print(f"Action sent: {action}")

    global epsilon
    epsilon = max(MIN_EPSILON, epsilon * EPSILON_DECAY)

stream = ZMQStream(socket)
stream.on_recv(process_request)
ioloop.IOLoop.instance().start()