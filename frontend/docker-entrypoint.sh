#!/bin/bash
./mo /usr/share/nginx/html/config.mo.js > /usr/share/nginx/html/config.js
exec "$@"