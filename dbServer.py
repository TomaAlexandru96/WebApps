#!/usr/bin/env python
 
from http.server import BaseHTTPRequestHandler, HTTPServer
 
class DBHTTPHandler(BaseHTTPRequestHandler):

  # PPOST
  def do_POST(self):
    print("Hello")

 
  # GET
  def do_GET(self):
    # Send response status code
    self.send_response(200)
    
    # Send headers
    self.send_header('Content-type','text/html')
    self.end_headers()
    
    # Send message back to client
    message = "Hello world!"
    # Write content as utf-8 data
    self.wfile.write(bytes(message, "utf8"))
    return

 
def run():
  print('starting server...')
 
  server_address = ('146.169.46.104', 8081)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  print('running server...')
  httpd.serve_forever()
 
 
run()
