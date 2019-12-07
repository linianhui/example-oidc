# https://hub.docker.com/_/traefik
FROM traefik:v2.1

COPY ./traefik/ /app

WORKDIR /app

EXPOSE 80 8080
