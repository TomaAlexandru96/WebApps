#!/usr/bin/python3

import psycopg2
import json
from urllib.parse import urlparse, parse_qs
from http.server import BaseHTTPRequestHandler, HTTPServer
 
NOT_FOUND = 404
OK = 200
NOT_ACCEPTABLE = 406

try:
  connect_str = "dbname='g1627107_u' user='g1627107_u'" + \
                "host='db.doc.ic.ac.uk' password='X44dCYOeJe'"
  # establish connection
  conn = psycopg2.connect(connect_str)
except Exception as e:
  print("Can't connect to psql!")
  print(e)


class DBHTTPHandler(BaseHTTPRequestHandler):

  # POST
  def do_POST(self):
    url = urlparse(self.path)
    data = self.rfile.read(int(self.headers['Content-Length'])).decode('utf-8')
    if (url.path == "/register"):
      self.handle_register(data)
    elif (url.path == "/room"):
      self.handle_post_room(data)
    elif (url.path == "/message"):
      self.handle_post_message(data)
    else:
      self.send_response(NOT_FOUND)

 
  # GET
  def do_GET(self):
    url = urlparse(self.path)
    if (url.path == "/users"):
      self.handle_users(url)
    elif (url.path == "/rooms"):
      self.handle_get_rooms(url)
    elif (url.path == "/messages"):
      self.handle_get_messages(url)
    else:
      self.send_response(NOT_FOUND)


  def send_JSON(self, obj):
    self.send_response(OK)
    self.send_header('Content-type','text/html')
    self.end_headers()
    self.wfile.write(bytes(json.dumps(obj), "utf8"))


  def handle_users(self, url):
    params = parse_qs(url.query)
    cursor = conn.cursor()
    query = '''SELECT *
               FROM USERS
               WHERE USERNAME = '{}' AND PASSWORD = '{}'
            '''.format(params['username'][0], params['password'][0])
    cursor.execute(query)
    response = cursor.fetchone()
    if (response is None):
      self.send_response(NOT_FOUND)
    else:
      user = {}
      user['id'] = response[0]
      user['username'] = response[1]
      user['password'] = response[2]
      user['email'] = response[3]
      self.send_JSON(user)
 
  
  def handle_register(self, userJSON):
    user = json.loads(userJSON)
    cursor = conn.cursor()
    query = '''SELECT *
               FROM USERS
               WHERE USERNAME = '{}'
            '''.format(user['username'])
    cursor.execute(query)

    if (cursor.fetchone() is not None):
      self.send_response(NOT_ACCEPTABLE)
      return

    cursor = conn.cursor()
    query = '''INSERT INTO USERS (USERNAME, PASSWORD, EMAIL) 
               VALUES ('{}', '{}', '{}')
            '''.format(user['username'], user['password'], user['email'])
    cursor.execute(query)
    conn.commit()
    self.send_JSON(user)


    def handle_get_rooms(self, url):
      pass

    
    def handle_get_messages(self, url):
      pass


    def handle_post_room(self, roomJSON):
      pass


    def handle_post_message(self, messageJSON):
      pass


def startServer():
  server_address = ('146.169.46.104', 8081)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  print ("Serving..")
  httpd.serve_forever()
 

if __name__ == "__main__":
  startServer()

