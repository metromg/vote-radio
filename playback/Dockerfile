FROM debian:stretch

ENV DEBIAN_FRONTEND noninteractive

# install liquidsoap
ENV PACKAGES "taglib mad lame vorbis cry samplerate liquidsoap"

RUN apt-get -qq -y update && \
    apt-get -y install \
    wget \
    opam \
    libmad0-dev \
    libmp3lame-dev \
    libogg-dev \
    libpcre3-dev \
    libsamplerate0-dev \
    libtag1-dev \
    libvorbis-dev \
    m4 \
    pkg-config && \
    rm -rf /var/lib/apt/lists/*

RUN groupadd liquidsoap --gid=888 && \
    useradd -m -g liquidsoap --uid=888 liquidsoap

USER liquidsoap

RUN opam init && \
    opam update && \
    eval `opam config env` && \
    opam depext $PACKAGES && \
    opam install $PACKAGES

USER root

RUN mkdir /var/log/liquidsoap && \
    chown liquidsoap:liquidsoap /var/log/liquidsoap

# install mustache
RUN wget -qO mo https://git.io/get-mo && \
    chmod +x mo

# copy liquidsoap configuration
RUN mkdir /etc/liquidsoap
COPY ./src /etc/liquidsoap/
RUN chown -R liquidsoap:liquidsoap /etc/liquidsoap

# copy entrypoint which will fill the configuration template with values from environment variables
COPY ./docker-entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

USER liquidsoap

VOLUME ["/var/log/liquidsoap"]
VOLUME ["/home/liquidsoap/music"]

ENTRYPOINT ["/entrypoint.sh"]
CMD ["/home/liquidsoap/.opam/system/bin/liquidsoap", "/etc/liquidsoap/liquidsoap.liq"]