# https://hub.docker.com/_/traefik
FROM traefik:1.7-alpine

COPY ./traefik/ /app

WORKDIR /app

EXPOSE 80 8080
