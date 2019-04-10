#!/usr/bin/python

import argparse
import httplib

parser = argparse.ArgumentParser(description="Fetches the next song filename from the radio backend.")
parser.add_argument("--dest", "-d", required=False, default="127.0.0.1")
parser.add_argument("--port", "-p", required=False, default="8080")
args = parser.parse_args()

try:
	timeout = 20
	conn = httplib.HTTPConnection(args.dest, int(args.port), timeout=timeout)
	conn.request("GET", "/next")
	result = conn.getresponse()
	if result.status == 200:
		next_song_filename = result.read()
		if not next_song_filename or len(next_song_filename) == 0:
			raise Exception("Got empty filename!")
		print "~/music/" + next_song_filename
	else:
		raise Exception("HTTP Error %s" % result.status)
	conn.close()
except Exception as e:
	if conn:
		conn.close()
	raise