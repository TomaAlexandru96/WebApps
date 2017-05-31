#!/usr/bin/python3

import psycopg2
import threading
import json
from urllib.parse import urlparse, parse_qs
from http.server import BaseHTTPRequestHandler, HTTPServer, SimpleHTTPRequestHandler
 
NOT_FOUND = 404
OK = 200
NOT_ACCEPTABLE = 406
CONFLICT = 409

try:
  connect_str = "dbname='g1627107_u' user='g1627107_u'" + \
                "host='db.doc.ic.ac.uk' password='X44dCYOeJe'"
  # establish connection
  conn = psycopg2.connect(connect_str)
except Exception as e:
  print("Can't connect to psql!")
  print(e)


def set_interval(func, sec):
  def func_wrapper():
      set_interval(func, sec) 
      func()  
  t = threading.Timer(sec, func_wrapper)
  t.start()
  return t


class DBHTTPHandler(BaseHTTPRequestHandler):

  # OPTIONS
  def do_OPTIONS(self):
    self.send_code_only(OK)


  # POST
  def do_POST(self):
    url = urlparse(self.path)
    data = self.rfile.read(int(self.headers['Content-Length'])).decode('utf-8')
    if (url.path == "/login"):
      self.handle_login(parse_qs(data))
    elif (url.path == "/register"):
      self.handle_register(parse_qs(data))
    elif (url.path == "/update_active_status"):
      self.handle_update_active_status(parse_qs(data))
    elif (url.path == "/request_friend"):
      self.handle_request_friend(parse_qs(data))
    elif (url.path == "/accept_friend_request"):
      self.handle_accept_friend_request(parse_qs(data))
    elif (url.path == "/reject_friend_request"):
      self.handle_reject_friend_request(parse_qs(data))
    else:
      self.send_code_only(NOT_FOUND);

 
  # GET
  def do_GET(self):
    url = urlparse(self.path)
    if (url.path == "/find_user"):
      self.handle_find_user(parse_qs(url.query))
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


  # login method
  def handle_login(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None or user['password'] != params['password'][0]):
      self.send_code_only(NOT_ACCEPTABLE)
    else:
      cursor = conn.cursor()
      query = '''UPDATE USERS SET ACTIVE = 't', LAST_TIME_ACTIVE = NOW()
                 WHERE USERNAME = '{}'
              '''.format(user['username'])
      cursor.execute (query)
      conn.commit()
      user['active'] = True
      self.send_JSON(user)

  
  def handle_update_active_status(self, data):
    user = self.helper_find_user(data['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    else:
      user['active'] = data['active'][0]
      cursor = conn.cursor()
      query = '''UPDATE USERS SET ACTIVE = '{}', LAST_TIME_ACTIVE = NOW()
                 WHERE USERNAME = '{}'
              '''.format(user['active'], user['username'])
      cursor.execute (query)
      conn.commit()
      self.send_JSON(user)


  def handle_find_user(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    else:
      self.send_JSON(user)


  def helper_find_user(self, u_name):
    cursor = conn.cursor()
    query = '''SELECT * 
               FROM USERS
               WHERE USERNAME = '{}'
            '''.format(u_name);
    cursor.execute(query)
    response = cursor.fetchone()
    if (response is None):
      return None
    else:
      user = {}
      user['username'] = response[0]
      user['password'] = response[1]
      user['email'] = response[2]
      user['friend_requests'] = response[3]
      user['active'] = response[4]
      user['friends'] = response[5]
      return user
 

  def handle_register(self, user):
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
    self.send_code_only(OK)


  def handle_request_friend(self, data):
    user = self.helper_find_user(data['user'][0]);
    requested_friend = self.helper_find_user(data['requested_friend'][0]);
    if (user is None or requested_friend is None):
      self.send_code_only(NOT_FOUND)
      return
  
    if (user['username'] == requested_friend['username']):
      self.send_code_only(CONFLICT)
      return

    
    if (user['username'] in requested_friend['friend_requests']):
      self.send_code_only(NOT_ACCEPTABLE)
      return    


    if (user['username'] in requested_friend['friends']):
      self.send_code_only(NOT_ACCEPTABLE)
      return    


    cursor = conn.cursor()
    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_prepend('{}', FRIEND_REQUESTS)
               WHERE USERNAME = '{}'
            '''.format(user['username'], requested_friend['username'])
    cursor.execute (query)
    conn.commit()
    self.send_code_only(OK)


  def handle_accept_friend_request(self, data):
    user = self.helper_find_user(data['user'][0]);
    requested_friend = self.helper_find_user(data['requested_friend'][0]);

    if (user is None or requested_friend is None):
      self.send_code_only(NOT_FOUND)
      return

    cursor = conn.cursor()
    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_remove(FRIEND_REQUESTS::varchar(50)[], ARRAY['{}']::varchar(50)[])
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    cursor.execute (query)
    conn.commit()

    cursor = conn.cursor()
    query = '''UPDATE USERS SET FRIENDS = array_prepend('{}', FRIENDS)
               WHERE USERNAME = '{}'
            '''.format(user['username'], requested_friend['username'])
    cursor.execute (query)
    conn.commit()

    cursor = conn.cursor()
    query = '''UPDATE USERS SET FRIENDS = array_prepend('{}', FRIENDS)
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    cursor.execute (query)
    conn.commit()
    self.send_code_only(OK)


  def handle_reject_friend_request(self, data):
    user = self.helper_find_user(data['user'][0]);
    requested_friend = self.helper_find_user(data['requested_friend'][0]);

    if (user is None or requested_friend is None):
      self.send_code_only(NOT_FOUND)
      return

    cursor = conn.cursor()
    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_remove(FRIEND_REQUESTS::varchar(50)[], ARRAY['{}']::varchar(50)[])
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    cursor.execute (query)
    conn.commit()
    self.send_code_only(OK)


def update_active_status():
  cursor = conn.cursor()
  query = '''UPDATE USERS SET ACTIVE = 'f' 
             WHERE EXTRACT(EPOCH FROM NOW() - LAST_TIME_ACTIVE) > 90'''
  cursor.execute (query)
  conn.commit()


def startServer():
  server_address = ('146.169.46.104', 8081)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  set_interval(update_active_status, 90)
  print("Serving..")
  httpd.serve_forever()
 

if __name__ == "__main__":
  startServer()

