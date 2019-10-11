# https://hub.docker.com/_/traefik
FROM traefik:v2.0

COPY ./traefik/ /app

WORKDIR /app

EXPOSE 80 8080
