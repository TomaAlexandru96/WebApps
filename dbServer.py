#!/usr/bin/python3

import psycopg2
 
from http.server import BaseHTTPRequestHandler, HTTPServer
 
class DBHTTPHandler(BaseHTTPRequestHandler):

  conn = None


  def connectToDB(self):
    if self.conn == None:
      try:
        connect_str = "dbname='g1627107_u' user='g1627107_u'" + \
                      "host='db.doc.ic.ac.uk' password='X44dCYOeJe'"
        # establish connection
        self.conn = psycopg2.connect(connect_str)
        return self.conn
      except Exception as e:
        print("Can't connect to psql!")
        print(e)
        return None
    return self.conn

  
  def run_query(self, message): 
    # create a psycopg2 cursor that can execute queries
    cursor = self.connectToDB().cursor()
    cursor.execute(message)
    return cursor


  # POST
  def do_POST(self):
    # Send response status code
    self.send_response(200)
    
    # Send headers
    self.send_header('Content-type','text/html')
    self.end_headers()
    
    # Send message back to client
    message = "You send a post request!"
    # Write content as utf-8 data
    self.wfile.write(bytes(message, "utf8"))
    return

 
  # GET
  def do_GET(self):
    # Send response status code
    self.send_response(200)
    
    # Send headers
    self.send_header('Content-type','text/html')
    self.end_headers()
    
    # Send message back to client
    message = "You send a get request!"
    # Write content as utf-8 data
    self.wfile.write(bytes(message, "utf8"))
    return

 
def startServer():
  print('starting server...')
 
  server_address = ('146.169.46.104', 8081)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  print('running server...')
  httpd.serve_forever()
 

if __name__ == "__main__":
  startServer()

