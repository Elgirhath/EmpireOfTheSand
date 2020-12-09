import zmq

context = zmq.Context()
socket = context.socket(zmq.PAIR)
socket.bind(f"tcp://*:5556")

# socket.recv() # waits for client empty frame

socket.send(b"Server message to client")
print("Sent")