import zmq
import sys
import time

port = 5556
context = zmq.Context()
socket = context.socket(zmq.PAIR)
socket.connect(f"tcp://localhost:{port}")

socket.send_string("")

msg = socket.recv()
print(msg)