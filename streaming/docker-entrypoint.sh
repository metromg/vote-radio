#!/bin/bash
mv /etc/icecast2/icecast.xml /etc/icecast2/icecast.xml.mo
./mo /etc/icecast2/icecast.xml.mo > /etc/icecast2/icecast.xml
rm /etc/icecast2/icecast.xml.mo
exec "$@"