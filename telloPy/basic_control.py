# This example script demonstrates how to send/receive commands to/from Tello
# This script is part of our course on Tello drone programming
# https://learn.droneblocks.io/p/tello-drone-programming-with-python/

# Import the built-in socket and time modules
import socket
import time

# IP and port of Tello
tello_address = ('192.168.10.1', 8889)

# Create a UDP connection that we'll send the command to
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock2 = socket.socket()

# Let's be explicit and bind to a local port on our machine where Tello can send messages
sock.bind(('', 9000))

# Function to send messages to Tello
def send(message):
  try:
    sock.sendto(message.encode(), tello_address)
    print("Sending message: " + message)
  except Exception as e:
    print("Error sending: " + str(e))

# Function that listens for messages from Tello and prints them to the screen
def receive():
  try:
    response, ip_address = sock.recvfrom(128)
    print("Received message: " + response.decode(encoding='utf-8') + " from Tello with IP: " + str(ip_address))
  except Exception as e:
    print("Error receiving: " + str(e))

def sleep(d):
  print(f"sleeping for {d}s...")
  x = 0
  while x < d:
    time.sleep(1)
    x += 1

# Send Tello into command mode
send("command")

# Receive response from Tello
receive()

# Get battery percentage from Tello
send("battery?")
receive()

# Takeoff
send("takeoff")
receive()
# send("up 50")
# receive()
# print("Successfully taken off...")

# sleep(3)

# Fly a 50cm sqare
# send("back 50")
# receive()
# send("left 50")
# receive()
# send("forward 50")
# receive()
# send("right 50")
# receive()

# sleep(3)

# Land
send("land")
receive()
print("land success")

# Close the UDP socket
sock.close()