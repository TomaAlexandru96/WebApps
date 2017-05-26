#!/usr/bin/python3

import psycopg2
import json
from urllib.parse import urlparse, parse_qs
from http.server import BaseHTTPRequestHandler, HTTPServer, SimpleHTTPRequestHandler
 
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

  # OPTIONS
  def do_OPTIONS(self):
    self.send_code_only(OK)


  # POST
  def do_POST(self):
    url = urlparse(self.path)
    data = self.rfile.read(int(self.headers['Content-Length'])).decode('utf-8')
    if (url.path == "/register"):
      self.handle_register(data)
    elif (url.path == "/set_active"):
      self.handle_set_active(data)
    else:
      self.send_code_only(NOT_FOUND);

 
  # GET
  def do_GET(self):
    url = urlparse(self.path)
    if (url.path == "/users"):
      self.handle_users(url)
    else:
      self.send_code_only(NOT_FOUND);


  def send_code_only (self, code):
    self.send_response(code)
    self.send_CORS()
    self.end_headers()


  def send_CORS (self):
    self.send_header("Access-Control-Allow-Credentials", "true")
    self.send_header("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time")
    self.send_header("Access-Control-Allow-Methods", "POST, GET, OPTIONS")
    self.send_header("Access-Control-Allow-Origin", "*")
    self.send_header("Access-Control-Max-Age", "86400")


  def send_JSON(self, obj):
    self.send_response(OK)
    self.send_CORS()
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
      self.send_code_only(NOT_FOUND)
    else:
      user = {}
      user['id'] = response[0]
      user['username'] = response[1]
      user['password'] = response[2]
      user['email'] = response[3]
      user['friends'] = response[4]
      user['friend_requests'] = response[5]
      user['active'] = response[6]
      self.send_JSON(user)
 
  
  def handle_register(self, userData):
    user = parse_qs(userData)
    cursor = conn.cursor()
    query = '''SELECT *
               FROM USERS
               WHERE USERNAME = '{}'
            '''.format(user['username'][0])
    cursor.execute(query)

    if (cursor.fetchone() is not None):
      self.send_code_only(NOT_ACCEPTABLE)
      return

    cursor = conn.cursor()
    query = '''INSERT INTO USERS (USERNAME, PASSWORD, EMAIL) 
               VALUES ('{}', '{}', '{}')
            '''.format(user['username'][0], user['password'][0], user['email'][0])
    cursor.execute(query)
    conn.commit()
    self.send_JSON(user)


  def handle_set_active(self, data):
    user = parse_qs(data)
    cursor = conn.cursor()
    query = '''UPDATE USERS SET ACTIVE = '{}'
               WHERE id = '{}'
            '''.format(user['active'][0], user['id'][0])
    cursor.execute (query)
    conn.commit()
    self.send_code_only(OK)

def startServer():
  server_address = ('146.169.46.104', 8081)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  print ("Serving..")
  httpd.serve_forever()
 

if __name__ == "__main__":
  startServer()

