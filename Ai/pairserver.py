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

while True:
    request = socket.recv()

    rq = Payload(request.decode('utf-8'))

    print(f"Request: {rq.UnitCount}")

    time.sleep(1) #calculate action

    action = random.choice([b"BuildSandStorage", b"BuildWaterStorage", b"BuildCastle"])

    socket.send(action)
    print(f"Action sent: {action}")