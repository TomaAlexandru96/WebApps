#!/usr/bin/python3

import ssl
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
    elif (url.path == "/logout"):
      self.handle_logout(parse_qs(data))
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
    elif (url.path == "/chooseCharacter"):
      self.handle_choose_character(parse_qs(data))
    elif (url.path == "/create_party"):
      self.handle_create_party(parse_qs(data))
    elif (url.path == "/join_party"):
      self.handle_join_party(parse_qs(data))
    elif (url.path == "/leave_party"):
      self.handle_leave_party(parse_qs(data))
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


  def send_db_query(self, query):
    cursor = conn.cursor()
    cursor.execute (query)
    if (not query.startswith("SELECT")):
      conn.commit()
    return cursor


  # login method
  def handle_login(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None or user['password'] != params['password'][0]):
      self.send_code_only(NOT_ACCEPTABLE)
    else:
      query = '''UPDATE USERS SET ACTIVE = 't', LAST_TIME_ACTIVE = NOW()
                 WHERE USERNAME = '{}'
              '''.format(user['username'])
      self.send_db_query(query)
      user['active'] = True
      self.send_JSON(user)


  def handle_logout(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    else:
      query = '''UPDATE USERS SET ACTIVE = 'f', LAST_TIME_ACTIVE = NOW()
                 WHERE USERNAME = '{}'
              '''.format(user['username'])
      self.send_db_query(query)
      
      if (not (user['party']['owner'] is None)):
        self.handle_leave_party({'username': [user['username']]})
      else:
        self.send_code_only(OK)


  def handle_create_party(self, data):
    user = self.helper_find_user(data['owner'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    elif (not (user['party']['owner'] is None)):
      self.send_code_only(NOT_ACCEPTABLE)
    else:
      query = '''INSERT INTO PARTIES (OWNER, MEMBERS, STATE)
		             VALUES ('{}', ARRAY['{}'], 1)
              '''.format(user['username'], user['username'])
      self.send_db_query(query)
      query = '''UPDATE USERS SET PARTY = '{}'
                 WHERE USERNAME = '{}'
              '''.format(user['username'], user['username'])
      self.send_db_query(query)
      self.send_code_only(OK)


  def handle_leave_party(self, data):
    user = self.helper_find_user(data['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    elif (user['party']['owner'] != user['username']):
      print ("smth")
      query = '''UPDATE PARTIES SET MEMBERS = array_remove(MEMBERS::varchar(50)[], ARRAY['{}']::varchar(50)[]) 
                 WHERE OWNER = '{}'
              '''.format(user['username'], user['party']['owner'])
      self.send_db_query(query)
      query = '''UPDATE USERS SET PARTY = NULL
                 WHERE USERNAME = '{}'
              '''.format(user['username'])
      self.send_db_query(query)
      self.send_code_only(OK)
    else:
      query = '''DELETE FROM PARTIES WHERE OWNER = '{}'
              '''.format(user['username'])
      self.send_db_query(query)
      for member in user['party']['partyMembers']:
        query = '''UPDATE USERS SET PARTY = NULL
                   WHERE USERNAME = '{}'
                '''.format(member)
        self.send_db_query(query)
      self.send_code_only(OK)


  def handle_join_party(self, data):
    owner = self.helper_find_user(data['owner'][0])
    user = self.helper_find_user(data['username'][0])
    if (user is None or owner is None):
      self.send_code_only(NOT_FOUND)
    else:
      query = '''UPDATE PARTIES SET MEMBERS = array_append(MEMBERS, '{}')
                 WHERE OWNER = '{}' 
              '''.format(user['username'], owner['username'])
      self.send_db_query(query)
      query = '''UPDATE USERS SET PARTY = '{}'
                 WHERE USERNAME = '{}'
              '''.format(owner['username'], user['username'])
      self.send_db_query(query)
      self.send_code_only(OK)


  def handle_update_active_status(self, data):
    user = self.helper_find_user(data['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    else:
      user['active'] = data['active'][0]
      query = '''UPDATE USERS SET ACTIVE = '{}', LAST_TIME_ACTIVE = NOW()
                 WHERE USERNAME = '{}'
              '''.format(user['active'], user['username'])
      self.send_db_query(query)
      self.send_JSON(user)


  def handle_find_user(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
    else:
      self.send_JSON(user)


  def handle_choose_character(self, params):
    user = self.helper_find_user(params['username'][0])
    if (user is None):
      self.send_code_only(NOT_FOUND)
      return

    query1 = '''
		INSERT INTO CHARACTERS (CHARACTER_NAME, CHARACTER_TYPE)
		VALUES ( '{}', '{}')
 		'''.format(params['characterName'][0], params['characterID'][0])
    self.send_db_query(query1)
    query2 = '''
		UPDATE USERS SET CHARACTER_NAME = '{}'
		WHERE USERNAME = '{}'
 		'''.format(params['characterName'][0], params['username'][0])
    self.send_db_query(query2)

    self.handle_find_user(params)


  def helper_find_user(self, u_name):
    query = '''SELECT *
               FROM USERS LEFT JOIN CHARACTERS ON USERS.CHARACTER_NAME = CHARACTERS.CHARACTER_NAME
               LEFT JOIN PARTIES ON USERS.PARTY = PARTIES.OWNER
               WHERE USERNAME = '{}'
            '''.format(u_name);
    response = self.send_db_query(query).fetchone()
    if (response is None):
      return None
    else:
      user = {}
      character = {}
      party = {}
      user['username'] = response[0]
      user['password'] = response[1]
      user['email'] = response[2]
      user['friend_requests'] = response[3]
      user['active'] = response[4]
      user['friends'] = response[5]
      user['character'] = character
      character['name'] = response[9]
      character['type'] = response[10]
      user['party'] = party
      party['owner'] = response[11]
      party['partyMembers'] = response[12]
      party['state'] = response[13]
      return user


  def handle_register(self, user):
    query = '''SELECT *
               FROM USERS
               WHERE USERNAME = '{}'
            '''.format(user['username'][0])
    cursor = self.send_db_query(query)
    
    if (cursor.fetchone() is not None):
      self.send_code_only(NOT_ACCEPTABLE)
      return

    query = '''INSERT INTO USERS (USERNAME, PASSWORD, EMAIL)
               VALUES ('{}', '{}', '{}')
            '''.format(user['username'][0], user['password'][0], user['email'][0])
    self.send_db_query(query)
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

    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_prepend('{}', FRIEND_REQUESTS)
               WHERE USERNAME = '{}'
            '''.format(user['username'], requested_friend['username'])
    self.send_db_query(query)
    self.send_code_only(OK)


  def handle_accept_friend_request(self, data):
    user = self.helper_find_user(data['user'][0]);
    requested_friend = self.helper_find_user(data['requested_friend'][0]);

    if (user is None or requested_friend is None):
      self.send_code_only(NOT_FOUND)
      return

    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_remove(FRIEND_REQUESTS::varchar(50)[], ARRAY['{}']::varchar(50)[])
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    self.send_db_query(query)
    query = '''UPDATE USERS SET FRIENDS = array_prepend('{}', FRIENDS)
               WHERE USERNAME = '{}'
            '''.format(user['username'], requested_friend['username'])
    self.send_db_query(query)
    query = '''UPDATE USERS SET FRIENDS = array_prepend('{}', FRIENDS)
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    self.send_db_query(query)
    self.send_code_only(OK)


  def handle_reject_friend_request(self, data):
    user = self.helper_find_user(data['user'][0]);
    requested_friend = self.helper_find_user(data['requested_friend'][0]);

    if (user is None or requested_friend is None):
      self.send_code_only(NOT_FOUND)
      return

    query = '''UPDATE USERS SET FRIEND_REQUESTS = array_remove(FRIEND_REQUESTS::varchar(50)[], ARRAY['{}']::varchar(50)[])
               WHERE USERNAME = '{}'
            '''.format(requested_friend['username'], user['username'])
    self.send_db_query(query)
    self.send_code_only(OK)


def update_active_status():
  cursor = conn.cursor()
  query = '''UPDATE USERS SET ACTIVE = 'f'
             WHERE EXTRACT(EPOCH FROM NOW() - LAST_TIME_ACTIVE) > 90'''
  cursor.execute (query)
  conn.commit()


def startServer():
  server_address = ('146.169.46.104', 8000)
  httpd = HTTPServer(server_address, DBHTTPHandler)
  httpd.socket = ssl.wrap_socket (httpd.socket, keyfile= '/tmp/crt.pem', certfile='/tmp/newcert.crt', server_side=True)
  set_interval(update_active_status, 90)
  print("Serving..")
  httpd.serve_forever()


if __name__ == "__main__":
  startServer()
