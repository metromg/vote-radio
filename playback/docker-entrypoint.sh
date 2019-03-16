#!/bin/bash
mv /etc/liquidsoap/liquidsoap.liq /etc/liquidsoap/liquidsoap.liq.mo
./mo /etc/liquidsoap/liquidsoap.liq.mo > /etc/liquidsoap/liquidsoap.liq
rm /etc/liquidsoap/liquidsoap.liq.mo
exec "$@"