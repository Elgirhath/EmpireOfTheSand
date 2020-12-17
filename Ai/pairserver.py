import zmq
import time
import json
import random

context = zmq.Context()
socket = context.socket(zmq.PAIR)
socket.bind(f"tcp://*:5556")
socket.recv() # waits for client empty frame

class Payload(object):
    def __init__(self, j):
        self.__dict__ = json.loads(j)

i = True

while True:
    request = socket.recv()

    rq = Payload(request.decode('utf-8'))

    print(f"Request: {rq.UnitCount}")

    time.sleep(10) #calculate action

    action = random.choice([b"BuildSandStorage", b"BuildWaterStorage", b"BuildCastle", b"Wait"])

    if i:
        socket.send(b"BuildCastle")
        time.sleep(10)
        i = False
    else:
        socket.send(action)
    print(f"Action sent: {action}")