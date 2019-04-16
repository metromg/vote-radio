#!/bin/bash
./mo /etc/liquidsoap/liquidsoap.mo.liq > /etc/liquidsoap/liquidsoap.liq
exec "$@"