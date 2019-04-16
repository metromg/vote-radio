#!/bin/bash
./mo /etc/icecast2/icecast.mo.xml > /etc/icecast2/icecast.xml
exec "$@"