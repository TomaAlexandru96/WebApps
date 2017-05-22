#!/usr/bin/python3

import psycopg2
 
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
 

def runPSQLQuery():
  try:
    connect_str = "dbname='g1627107_u' user='g1627107_u' host='db.doc.ic.ac.uk'" + \
                  "password='X44dCYOeJe'"
    # establish connection
    conn = psycopg2.connect(connect_str)
    # create a psycopg2 cursor that can execute queries
    cursor = conn.cursor()

    # create a new table with a single column called "name"
    #cursor.execute("""CREATE TABLE tutorials (name char(40));""")
    # run a SELECT statement - no data in there, but we can try it
    #cursor.execute("""SELECT * from tutorials""")
    #rows = cursor.fetchall()
    #print(rows)
  except Exception as e:
    print("Can't connect to psql!")
    print(e)
 
# run()
runPSQLQuery()

