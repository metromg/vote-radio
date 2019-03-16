FROM debian:stretch

ENV DEBIAN_FRONTEND noninteractive

# install icecast
RUN apt-get -qq -y update && \
    apt-get -y install \
    wget \
    gnupg && \
    wget -qO - http://icecast.org/multimedia-obs.key | apt-key add -

RUN echo "deb http://download.opensuse.org/repositories/multimedia:/xiph/Debian_9.0/ ./" >> /etc/apt/sources.list.d/icecast.list

RUN apt-get -qq -y update && \
    apt-get -qq -y install icecast2 && \
    rm -rf /var/lib/apt/lists/*

# install mustache
RUN wget -qO mo https://git.io/get-mo && \
    chmod +x mo

# copy icecast configuration
COPY ./src /etc/icecast2/
RUN chown -R icecast2:icecast /etc/icecast2

# copy entrypoint which will fill the configuration template with values from environment variables
COPY ./docker-entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

USER icecast2

EXPOSE 8000
VOLUME ["/var/log/icecast2"]

ENTRYPOINT ["/entrypoint.sh"]
CMD ["icecast2", "-c", "/etc/icecast2/icecast.xml"]