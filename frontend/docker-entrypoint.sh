#!/bin/bash
./mo /usr/share/nginx/html/config.mo > /usr/share/nginx/html/config.js
rm /usr/share/nginx/html/config.mo
exec "$@"